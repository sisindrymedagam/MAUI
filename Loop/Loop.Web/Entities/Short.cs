using System.ComponentModel.DataAnnotations;

namespace Loop.Web.Entities;

public class Short
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Name { get; set; } = string.Empty;

    [Required]
    public string Type { get; set; } = string.Empty;

    public long Size { get; set; }

    [Required]
    public string URL { get; set; } = string.Empty;

    public DateTime CreatedOn { get; set; }

    public string? CreatedBy { get; set; }
}