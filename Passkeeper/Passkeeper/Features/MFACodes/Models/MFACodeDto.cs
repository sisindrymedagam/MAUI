using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Passkeeper.Features.MFACodes.Models;

[Table("MFACodes")]
public class MFACodeDto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    //Username
    [Required]
    public string AccountName { get; set; }

    //Username
    [Required]
    public string AccountSecret { get; set; }

    //Code issuer
    [Required]
    public string Issuer { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }

    //public string Code { get; set; }

    //public int Ticks { get; set; }
}