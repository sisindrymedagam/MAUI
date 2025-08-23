using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Looply.MAUI.Models;

public class AuthResponse
{
    public string Token { get; set; } = default!;

    public DateTime Expiration { get; set; }
}

public class SyncViewModel<T>
{
    public DateTime ServerSyncTime { get; set; }

    public List<T> Updates { get; set; } = [];

    public List<int> Deletes { get; set; } = [];
}

public class ShortsListDto
{
    [PrimaryKey]
    public int Id { get; set; }

    [Required]
    [Display(Name = "Short Name")]
    public string Name { get; set; } = string.Empty;

    public string? Type { get; set; }

    public long? Size { get; set; }

    public string? URL { get; set; }

    public string? SizeInMB { get; set; }
}