using System.ComponentModel.DataAnnotations;

namespace SupportFlow.Api.DTOs.Auth;

public class LoginDto
{
    [Required]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public string Email {get; set;} = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string Password {get; set;} = string.Empty;
}