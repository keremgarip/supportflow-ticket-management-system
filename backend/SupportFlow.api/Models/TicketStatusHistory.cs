namespace SupportFlow.Api.Models;

public class TicketStatusHistory
{
    public int Id {get; set;}
    public int TicketId {get; set;}
    public string? OldStatus {get; set;}
    public string NewStatus {get; set;} = string.Empty;
    public int ChangedByUserId {get; set;}
    public DateTime ChangedAt {get; set;} = DateTime.UtcNow;
    public Ticket Ticket {get; set;} = null!;
    public User ChangedByUser {get; set;} = null!;
}