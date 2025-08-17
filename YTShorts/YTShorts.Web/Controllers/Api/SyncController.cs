using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YTShorts.Web.Data;
using YTShorts.Web.Models;

namespace YTShorts.Web.Controllers.Api;

[Route("api/[controller]")]
[ApiController]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class SyncController(ApplicationDbContext context) : ControllerBase
{
    /// <summary>
    /// Retrieves Shorts updates and deletes since the last synchronization.
    /// </summary>
    /// <param name="lastSync">
    /// The timestamp of the last successful synchronization from the client.  
    /// If null, all records will be returned (initial sync).
    /// </param>
    /// <returns>
    /// A <see cref="SyncViewModel{ShortsListViewModel}"/> object containing:
    /// <list type="bullet">
    /// <item><description>ServerSyncTime → the server's current UTC time.</description></item>
    /// <item><description>Updates → list of Shorts created/updated since <paramref name="lastSync"/>.</description></item>
    /// <item><description>Deletes → list of Shorts IDs deleted since <paramref name="lastSync"/>.</description></item>
    /// </list>
    /// </returns>
    /// <response code="200">Returns updates and deletes for the client to apply.</response>
    [HttpGet]
    public IActionResult Index([FromQuery] DateTime? lastSync)
    {
        var syncViewModel = new SyncViewModel<ShortsListViewModel>
        {
            ServerSyncTime = DateTime.UtcNow,
            Updates = context.Shorts.Where(s => lastSync == null || s.CreatedOn > lastSync).AsNoTracking()
            .Select(s => new ShortsListViewModel
            {
                Id = s.Id,
                Name = s.Name,
                Type = s.Type,
                Size = s.Size,
                URL = s.URL
            }).ToList(),
            Deletes = context.DeletedContents.Where(s => lastSync == null || s.DeletedOn > lastSync)
            .Select(s => s.DeletedContentId).ToList()
        };

        return Ok(syncViewModel);
    }
}
