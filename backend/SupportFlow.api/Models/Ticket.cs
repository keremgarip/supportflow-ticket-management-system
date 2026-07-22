using System.ComponentModel.DataAnnotations.Schema;

namespace SupportFlow.Api.Models;

public class Ticket
{
    public int Id {get; set; }
    public string Title {get; set;} = string.Empty;
    public string Description {get; set;} = string.Empty;
    public string Status {get; set;} = "Open";
    public string Priority {get; set;} = "Medium";

    public int CategoryId {get; set;}
    public int CustomerId {get; set;}

    public int? AssignedAgentId {get; set;}
    public DateTime CreatedAt {get; set;} = DateTime.UtcNow;
    public DateTime UpdatedAt {get; set;} = DateTime.UtcNow;
    public DateTime? ClosedAt {get; set;}

    public TicketCategory Category{get; set;} = null!;
    public User Customer{get; set;} = null!;

    public User? AssignedAgent {get; set;}
    
    public ICollection<TicketMessage> Messages {get; set;} = new List<TicketMessage>();

    public ICollection<TicketStatusHistory> StatusHistories {get; set;} = new List<TicketStatusHistory>();

    public ICollection<TicketAttachment> Attachments {get; set;} = new List<TicketAttachment>();

    public ICollection<Notification> Notifications {get; set;} = new List<Notification>();
}