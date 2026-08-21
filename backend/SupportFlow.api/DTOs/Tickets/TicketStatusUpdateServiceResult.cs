using SupportFlow.Api.Helpers;

namespace SupportFlow.Api.DTOs.Tickets;

public class TicketStatusUpdateServiceResult
{
    public TicketStatusUpdateStatus Status {get; set;}
    public TicketStatusUpdateResultDto? Result {get; set;}
    public string? CurrentStatus {get; set;}
    public string? RequestedStatus {get; set;}
}