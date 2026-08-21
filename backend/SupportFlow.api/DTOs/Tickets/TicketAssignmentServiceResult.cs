using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.DTOs.Tickets;

public class TicketAssignmentServiceResult
{
    public TicketAssignmentStatus Status {get; set;}
    public TicketAssignmentResultDto? Assignment {get; set;}
}