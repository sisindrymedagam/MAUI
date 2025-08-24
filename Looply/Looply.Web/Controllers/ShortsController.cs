using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Looply.Web.Data;
using Looply.Web.Entities;
using Looply.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Looply.Web.Controllers;

[Authorize]
public class ShortsController : Controller
{
    private readonly ApplicationDbContext _context;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _containerName;
    private readonly IConfiguration _configuration;
    private readonly ILogger<HomeController> _logger;

    public ShortsController(ApplicationDbContext context,
        BlobServiceClient blobServiceClient,
        IConfiguration configuration, ILogger<HomeController> logger)
    {
        _context = context;
        _blobServiceClient = blobServiceClient;
        _configuration = configuration;
        _logger = logger;
        _containerName = _configuration["BlobStorage:ContainerName"] ?? "shortsvideos";
    }

    // GET: Shorts
    public async Task<IActionResult> Index()
    {
        return View(await _context.Shorts.Select(s => new ShortsListDto
        {
            Name = s.Name,
            Id = s.Id,
            Size = s.Size,
            Type = s.Type,
            URL = s.URL
        }).ToListAsync());
    }

    // GET: Shorts/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();
        ShortDetailsDto? shortDetails = await _context.Shorts.Select(s => new ShortDetailsDto
        {
            Id = s.Id,
            Name = s.Name,
            Size = s.Size,
            Type = s.Type,
            URL = s.URL,
            CreatedBy = s.CreatedBy,
            CreatedOn = s.CreatedOn,
        }).FirstOrDefaultAsync(s => s.Id == id);

        return shortDetails == null ? NotFound() : View(shortDetails);
    }

    // GET: Shorts/Create
    public IActionResult Create()
    {
        return View(new CreateShortDto());
    }

    // POST: Shorts/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateShortDto model)
    {
        const int maxFiles = 5;
        const long maxFileSize = 5 * 1024 * 1024; // 5MB

        if (model.Files == null || model.Files.Length == 0)
        {
            ModelState.AddModelError("Files", "Please upload at least one video file.");
        }
        else if (model.Files.Length > maxFiles)
        {
            ModelState.AddModelError("Files", $"You can upload a maximum of {maxFiles} files at once.");
        }
        else if (model.Files.Any(f => !f.ContentType.StartsWith("video/")))
        {
            ModelState.AddModelError("Files", "Only video files are allowed.");
        }
        else if (model.Files.Any(f => f.Length > maxFileSize))
        {
            ModelState.AddModelError("Files", $"Each file must be less than or equal to 5 MB.");
        }

        if (ModelState.IsValid)
        {
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
            foreach (IFormFile file in model.Files)
            {
                string blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                BlobClient blobClient = containerClient.GetBlobClient(blobName);
                using (Stream stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
                }
                Short shorts = new()
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    URL = blobClient.Uri.ToString(),
                    Size = file.Length,
                    Type = file.ContentType,
                    CreatedOn = DateTime.UtcNow,
                    CreatedBy = User.Identity?.Name
                };
                _context.Add(shorts);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(model);
    }

    // POST: Shorts/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        Short? shorts = await _context.Shorts.FindAsync(id);
        if (shorts != null)
        {
            Uri blobUri = new(shorts.URL);
            string blobName = Path.GetFileName(blobUri.LocalPath);
            BlobContainerClient containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
            BlobClient blobClient = containerClient.GetBlobClient(blobName);
            await blobClient.DeleteIfExistsAsync();

            _context.Shorts.Remove(shorts);

            await _context.DeletedContents.AddAsync(new DeletedContent
            {
                DeletedContentId = shorts.Id,
                DeletedOn = DateTime.UtcNow,
                DeletedBy = User.Identity?.Name
            });

            await _context.SaveChangesAsync();
        }
        return RedirectToAction(nameof(Index));
    }
}
