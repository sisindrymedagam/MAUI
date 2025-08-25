using Loop.Web.Data;
using Loop.Web.Entities;
using Loop.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Loop.Web.Controllers;

[Authorize]
public class ShortsController : Controller
{
    private readonly string FileUploadPath;
    private readonly string BaseUrl;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _env;

    public ShortsController(ApplicationDbContext context,
        IWebHostEnvironment env, IConfiguration configuration)
    {
        _context = context;
        _env = env;
        _configuration = configuration;
        FileUploadPath = _configuration["FileUpload:FolderPath"] ?? "uploads";
        BaseUrl = _configuration["FileUpload:ServerBaseUrl"];
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
            foreach (IFormFile file in model.Files)
            {
                var path = await UploadFileAsync(file);

                Short shorts = new()
                {
                    Name = Path.GetFileNameWithoutExtension(file.FileName),
                    URL = path,
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
            var pathOnly = shorts.URL.Replace(BaseUrl, "").TrimStart('/');
            var fullPath = Path.Combine(_env.WebRootPath, pathOnly);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

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

    private async Task<string> UploadFileAsync(IFormFile file)
    {
        var uploadPath = Path.Combine(_env.WebRootPath, FileUploadPath);
        if (!Directory.Exists(uploadPath))
            Directory.CreateDirectory(uploadPath);

        var uniqueFileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
        var fullPath = Path.Combine(uploadPath, uniqueFileName);

        using (var stream = new FileStream(fullPath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        // Return relative path (URL to serve from browser)
        return $"{BaseUrl}/{FileUploadPath}/{uniqueFileName}";
    }

}
