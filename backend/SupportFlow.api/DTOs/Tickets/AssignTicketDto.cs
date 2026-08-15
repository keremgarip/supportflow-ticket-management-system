using System.ComponentModel.DataAnnotations;

namespace SupportFlow.Api.DTOs.Tickets;

public class AssignTicketDto
{
    [Range(
        1,
        int.MaxValue,
        ErrorMessage = "AgentId must be greater than zero."
    )]
    public int AgentId {get; set;}
}