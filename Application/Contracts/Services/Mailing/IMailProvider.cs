namespace Application.Contracts.Services.Mailing;

public interface IMailProvider
{
    Task<bool> Send(string from, string fromName, string to, string toName, int? templateId,
        object? templateParams = null, IDictionary<string, Stream>? attachments = null, string subject = null,
        string content = null);

}