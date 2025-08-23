using Looply.Web.Enums;
using System.ComponentModel.DataAnnotations;

namespace Looply.Web.Entities;

public class DeletedContent
{
    [Key]
    public int Id { get; set; }

    [Required]
    public ContentTypeEnum Type { get; set; }

    [Required]
    public int DeletedContentId { get; set; }

    public DateTime DeletedOn { get; set; }

    public string? DeletedBy { get; set; }
}
