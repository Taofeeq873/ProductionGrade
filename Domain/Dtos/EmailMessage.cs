namespace Domain.Dtos;

public class EmailMessage
{
    public EmailMessage()
    {
        Attachments = new Dictionary<string, Stream>();
    }

    public string From { get; set; }
    public string FromName { get; set; }
    public string To { get; set; }
    public string ToName { get; set; }
    public int? TemplateId { get; set; }
    public object? TemplateParams { get; set; }
    public IDictionary<string, Stream> Attachments { get; set; }
    public string? Subject { get; set; }
    public string? Content { get; set; }
}