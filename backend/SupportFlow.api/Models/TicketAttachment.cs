namespace SupportFlow.Api.Models;

public class TicketAttachment
{
    public int Id {get; set;}
    public int TicketId {get; set;}
    public int UploadedByUserId {get; set;}
    public string FileName {get; set;} = string.Empty;
    public string FilePath {get; set;} = string.Empty;
    public string ContentType {get; set;} = string.Empty;
    public DateTime UploadedAt {get; set;} = DateTime.UtcNow;
    public Ticket Ticket {get; set;} = null!;
    public User UploadedByUser {get; set;} = null!;
}