using System.ComponentModel.DataAnnotations;

namespace Loop.Web.Models;

public class LoginDto
{
    [Required(ErrorMessage = "Email is required.", AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}
