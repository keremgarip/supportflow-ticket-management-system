using System.ComponentModel.DataAnnotations;
using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.DTOs.Auth;

public class RegisterDto
{
    [Required]
    [NotWhiteSpace(ErrorMessage = "Full name cannot be empty or whitespace.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 150 characters.")]
    public string FullName {get; set;} = string.Empty;
    [Required]
    [EmailAddress(ErrorMessage = "A valid email address is required.")]
    [StringLength(255, ErrorMessage = "Email cannot exceed 255 characters.")]
    public string Email {get; set;} = string.Empty;
    [Required]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string Password {get; set;} = string.Empty;
    [Required]
    [Compare(nameof(Password), ErrorMessage = "Password and confirmation password do not match.")]
    public string ConfirmPassword {get; set;} = string.Empty;
}