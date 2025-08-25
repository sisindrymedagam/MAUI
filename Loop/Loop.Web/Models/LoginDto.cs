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

public class RegisterDto
{
    [Required(ErrorMessage = "Name is required.", AllowEmptyStrings = false)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.", AllowEmptyStrings = false)]
    [DataType(DataType.EmailAddress)]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;
}

public class UserListDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
}

public class ChangePasswordDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "New Password is required.", AllowEmptyStrings = false)]
    [DataType(DataType.Password)]
    public string NewPassword { get; set; } = string.Empty;
}