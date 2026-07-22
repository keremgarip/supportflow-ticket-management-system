namespace SupportFlow.Api.Models;

public class TicketMessage
{
    public int Id {get; set;}
    public int TicketId {get; set;}
    public int SenderId {get; set;}
    public string Message {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public Ticket Ticket {get; set;} = null!;
    public User Sender {get; set;} = null!;
}