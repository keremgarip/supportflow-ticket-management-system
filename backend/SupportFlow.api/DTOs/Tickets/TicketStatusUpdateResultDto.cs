namespace SupportFlow.Api.DTOs.Tickets;

public class TicketStatusUpdateResultDto
{
    public int TicketId {get; set;}
    public string PreviousStatus {get; set;}
    public string currentStatus {get; set;}
    public DateTime UpdatedAt {get; set;}
}