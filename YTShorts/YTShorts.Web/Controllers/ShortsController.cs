using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YTShorts.Web.Data;
using YTShorts.Web.Models;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System;
using System.Linq;
using System.IO;
// Add Azure Blob Storage namespaces
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Configuration;

namespace YTShorts.Web.Controllers
{
    public class ShortsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName = "shortsvideos";

        public ShortsController(ApplicationDbContext context, BlobServiceClient blobServiceClient)
        {
            _context = context;
            _blobServiceClient = blobServiceClient;
        }

        // GET: Shorts
        public async Task<IActionResult> Index()
        {
            return View(await _context.Shorts.ToListAsync());
        }

        // GET: Shorts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var shorts = await _context.Shorts.FindAsync(id);
            if (shorts == null) return NotFound();
            return View(shorts);
        }

        // GET: Shorts/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shorts/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] Shorts shorts, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please upload a video file.");
            }
            else if (!file.ContentType.StartsWith("video/"))
            {
                ModelState.AddModelError("file", "Only video files are allowed.");
            }

            if (ModelState.IsValid)
            {
                // Upload to Azure Blob Storage
                var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
                await containerClient.CreateIfNotExistsAsync(PublicAccessType.Blob);
                var blobName = Guid.NewGuid() + Path.GetExtension(file.FileName);
                var blobClient = containerClient.GetBlobClient(blobName);
                using (var stream = file.OpenReadStream())
                {
                    await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = file.ContentType });
                }
                shorts.URL = blobClient.Uri.ToString();
                shorts.Size = file.Length;
                shorts.Type = file.ContentType;
                shorts.CreatedOn = DateTime.UtcNow;
                shorts.CreatedBy = User.Identity?.Name;
                _context.Add(shorts);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shorts);
        }

        // GET: Shorts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var shorts = await _context.Shorts.FindAsync(id);
            if (shorts == null) return NotFound();
            return View(shorts);
        }

        // POST: Shorts/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Type,Size,URL,CreatedOn,CreatedBy")] Shorts shorts)
        {
            if (id != shorts.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    shorts.UpdatedOn = DateTime.UtcNow;
                    shorts.UpdatedBy = User.Identity?.Name;
                    _context.Update(shorts);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Shorts.Any(e => e.Id == shorts.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(shorts);
        }

        // GET: Shorts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var shorts = await _context.Shorts.FindAsync(id);
            if (shorts == null) return NotFound();
            return View(shorts);
        }

        // POST: Shorts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shorts = await _context.Shorts.FindAsync(id);
            if (shorts != null)
            {
                _context.Shorts.Remove(shorts);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
} 