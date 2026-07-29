using System.ComponentModel.DataAnnotations;

namespace SupportFlow.Api.DTOs.Categories;

public class CreateCategoryDto
{
    [Required]
    [StringLength(
        100,
        MinimumLength = 2,
        ErrorMessage = "Category name must be between 2 and 100 characters."
    )]
    public string Name {get; set;} = string.Empty;

    [StringLength(
        500,
        ErrorMessage = "Description cannot exceed 500 characters."
    )]
    public string? Description {get; set;}
}