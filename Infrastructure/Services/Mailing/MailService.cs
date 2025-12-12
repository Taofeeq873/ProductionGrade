using System.Text.Json;
using System.Text;
using Application.Contracts.Services.Mailing;
using Application.Contracts.Services.Queue;
using Domain.Configurations;
using Domain.Dtos;
using Domain.Entities;
using Application.Dtos;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Policy;

namespace Infrastructure.Services.Mailing;

public class MailService : IMailService
{
    private readonly ILogger<MailService> _logger;
    private readonly MailConfiguration _mailingConfiguration;
    private readonly IProducer _producer;

    public MailService(
        IProducer producer,
        IOptions<MailConfiguration> options,
        ILogger<MailService> logger)
    {
        _producer = producer;
        _mailingConfiguration = options.Value;
        _logger = logger;
    }
   

    public async Task<bool> SendMail(string email, string name, string subject, string content)
    {
        var mail = CreateEmailMessage(
            email,
            name,
            null,
            null,
            subject, content
        );
        return await PublishEmailMessageToQueue(mail);
    }

    private EmailMessage CreateEmailMessage(string to, string toName, int? templateId = null,
        object? templateParams = null, string? subject = null, string? content = null,
        IDictionary<string, Stream>? attachments = null)
    {
        return new EmailMessage
        {
            From = _mailingConfiguration.FromEmail,
            FromName = _mailingConfiguration.FromName,
            To = to,
            ToName = toName,
            TemplateId = templateId,
            TemplateParams = templateParams,
            Subject = subject,
            Content = content,
            Attachments = attachments ?? new Dictionary<string, Stream>()
        };
    }

    private async Task<bool> PublishEmailMessageToQueue(EmailMessage emailMessage)
    {
        try
        {
            var jsonData = JsonSerializer.Serialize(emailMessage);
            _producer.SendToQueue(jsonData, "emailQueue");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing email to queue");
            return false;
        }
    }
}