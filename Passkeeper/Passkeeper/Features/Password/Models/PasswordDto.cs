using SQLite;
using System.ComponentModel.DataAnnotations;

namespace Passkeeper.Features.Password.Models;

[Table("Passwords")]
public class PasswordDto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Required]
    public string Url { get; set; }

    [Required]
    public string Username { get; set; }

    [Required]
    public string Password { get; set; }

    public string? Note { get; set; }

    public string? Company { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public DateTime UpdatedAtUtc { get; set; }
}