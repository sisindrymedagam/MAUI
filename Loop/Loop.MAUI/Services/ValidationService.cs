using System.Text.RegularExpressions;

namespace Loop.MAUI.Services;

public interface IValidationService
{
    ValidationResult ValidateEmail(string email);
    ValidationResult ValidatePassword(string password);
    ValidationResult ValidateVideoUrl(string url);
}

public class ValidationService : IValidationService
{
    private static readonly Regex EmailRegex = new(
        @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    private static readonly Regex UrlRegex = new(
        @"^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public ValidationResult ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return ValidationResult.Failure("Email is required");

        if (email.Length > 254)
            return ValidationResult.Failure("Email is too long");

        if (!EmailRegex.IsMatch(email))
            return ValidationResult.Failure("Invalid email format");

        return ValidationResult.Success();
    }

    public ValidationResult ValidatePassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            return ValidationResult.Failure("Password is required");

        if (password.Length < 6)
            return ValidationResult.Failure("Password must be at least 6 characters long");

        if (password.Length > 128)
            return ValidationResult.Failure("Password is too long");

        // Check for at least one letter and one number
        bool hasLetter = password.Any(char.IsLetter);
        bool hasNumber = password.Any(char.IsDigit);

        if (!hasLetter || !hasNumber)
            return ValidationResult.Failure("Password must contain at least one letter and one number");

        return ValidationResult.Success();
    }

    public ValidationResult ValidateVideoUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return ValidationResult.Failure("Video URL is required");

        if (!UrlRegex.IsMatch(url))
            return ValidationResult.Failure("Invalid video URL format");

        // Check for common video file extensions
        string[] validExtensions = { ".mp4", ".avi", ".mov", ".wmv", ".flv", ".webm", ".mkv" };
        bool hasValidExtension = validExtensions.Any(ext => 
            url.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

        if (!hasValidExtension)
            return ValidationResult.Failure("URL must point to a valid video file");

        return ValidationResult.Success();
    }
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;

    public static ValidationResult Success() => new() { IsValid = true };
    public static ValidationResult Failure(string message) => new() { IsValid = false, ErrorMessage = message };
}
