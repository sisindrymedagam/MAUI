using System.ComponentModel.DataAnnotations;

namespace Loop.Web.Models;

public class ShortDetailsDto : ShortsListDto
{
    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public string? UpdatedBy { get; set; }
}

public class CreateShortDto
{
    [Display(Name = "Video Files")]
    [Required(ErrorMessage = "Please upload at least one video file.")]
    public IFormFile[] Files { get; set; } = Array.Empty<IFormFile>();
}

public class ShortsListDto
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Short Name")]
    public string Name { get; set; } = string.Empty;

    public string? Type { get; set; }

    public long? Size { get; set; }

    public string? URL { get; set; }

    // Returns the size in megabytes (MB), rounded to 1 decimal place, with " MB" appended, or null if Size is null
    public string? SizeInMB => Size.HasValue ? $"{Math.Round((double)Size.Value / (1024 * 1024), 1)} MB" : null;
}