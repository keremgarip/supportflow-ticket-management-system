namespace SupportFlow.Api.DTOs.Messages;

public class TicketMessageDto
{
    public int Id {get; set;}
    public string SenderName {get; set;} = string.Empty;
    public string SenderRole {get; set;} = string.Empty;
    public string Message {get; set;} = string.Empty;
    public DateTime CreatedAt {get; set;}
}