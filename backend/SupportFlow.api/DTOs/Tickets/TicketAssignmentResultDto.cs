namespace SupportFlow.Api.DTOs.Tickets;

public class TicketAssignmentResultDto
{
    public int TicketId {get; set;}
    public int AgentId {get; set;}
    public string AgentName {get; set;} = string.Empty;
    public string PreviousStatus {get; set;} = string.Empty;
    public string CurrentStatus {get; set;} = string.Empty;
    public DateTime UpdatedAt {get; set;}
}