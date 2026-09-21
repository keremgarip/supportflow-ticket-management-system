using System.ComponentModel.DataAnnotations;
using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.DTOs.Messages;

public class CreateTicketMessageDto
{
    [Required]
    [NotWhiteSpace(
        ErrorMessage = "Message cannot be empty or whitespace."
    )]
    [StringLength(
        5000,
        MinimumLength = 1,
        ErrorMessage =
            "Message must be between 1 and 5000 characters."
    )]
    public string Message {get; set;} = string.Empty;
}