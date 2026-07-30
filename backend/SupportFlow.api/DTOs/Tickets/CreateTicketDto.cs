using System.ComponentModel.DataAnnotations;
using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.DTOs.Tickets;

public class CreateTicketDto
{
    [Required]
    [NotWhiteSpace(
        ErrorMessage = "Ticket title cannot be empty or whitespace.")]
    [StringLength(
        200,
        MinimumLength = 5,
        ErrorMessage = "Ticket title must be between 5 and 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [NotWhiteSpace(
        ErrorMessage = "Ticket description cannot be empty or whitespace.")]
    [StringLength(
        5000,
        MinimumLength = 10,
        ErrorMessage =
            "Ticket description must be between 10 and 5000 characters.")]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Helpers.AllowedValues(
        "Low",
        "Medium",
        "High",
        "Critical",
        ErrorMessage =
            "Priority must be Low, Medium, High or Critical.")]
    public string Priority { get; set; } = "Medium";

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "CategoryId must be greater than zero.")]
    public int CategoryId { get; set; }

    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "CustomerId must be greater than zero.")]
    public int CustomerId { get; set; }
}