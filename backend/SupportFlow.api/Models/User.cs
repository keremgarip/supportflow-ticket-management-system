namespace SupportFlow.Api.Models;

public class User
{
    public int Id {get; set;}

    public string FullName {get; set;} = string.Empty;

    public string Email {get; set;} = string.Empty;

    public string PasswordHash {get; set;} = string.Empty;

    public string Role {get; set;} = "Customer";

    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;

    public bool IsActive {get; set;} = true;

    public ICollection<Ticket> CreatedTickets {get; set;} = new List<Ticket>();
    public ICollection<Ticket> AssignedTickets {get; set;} = new List<Ticket>();

    public ICollection<TicketMessage> SentMessages {get; set;} = new List<TicketMessage>();

    public ICollection<TicketStatusHistory> StatusChanges {get; set;} = new List<TicketStatusHistory>();

    public ICollection<TicketAttachment> UploadedAttachments {get; set;} = new List<TicketAttachment>();

    public ICollection<Notification> Notifications {get; set;} = new List<Notification>(); 
}