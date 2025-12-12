using Application.Contracts.Services.Mailing;
using Domain.Configurations;
using Infrastructure.Exceptions.Mailing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;

namespace Infrastructure.Services.Mailing;

public class MailProvider : IMailProvider
{
    private readonly TransactionalEmailsApi _emailApi;
    private readonly ILogger<MailProvider> _logger;
    private readonly MailConfiguration _mailConfiguration;

    public MailProvider(ILogger<MailProvider> logger, IOptions<MailConfiguration> options)
    {
        _logger = logger;
        _mailConfiguration = options.Value;

        Configuration.Default.ApiKey.Add("api-key", _mailConfiguration.ApiKey);
        _emailApi = new TransactionalEmailsApi();
    }

    
    public async Task<bool> Send(string from, string fromName, string to, string toName,
        int? templateId, object? templateParams = null, IDictionary<string, Stream>? attachments = null,
        string subject = null, string content = null)
    {
        try
        {
            var sendSmtpEmail = new SendSmtpEmail
            {
                Sender = new SendSmtpEmailSender(fromName, from),
                To = new List<SendSmtpEmailTo> { new(to, toName) },
                Subject = subject ?? "ProductGrade Mail" // ✅ Fallback subject
            };

            // ✅ If template is being used
            if (templateId is not null and > 0)
            {
                sendSmtpEmail.TemplateId = templateId;
                sendSmtpEmail.Params = templateParams;
            }
            else
            {
                sendSmtpEmail.HtmlContent = content;
            }

            if (attachments != null && attachments.Any())
                sendSmtpEmail.Attachment = attachments.Select(a => new SendSmtpEmailAttachment(
                    name: a.Key,
                    content: ReadFully(a.Value)
                )).ToList();

            var response = await _emailApi.SendTransacEmailAsync(sendSmtpEmail);
            _logger.LogInformation("Email sent successfully: {0}", response.MessageId);
            return true;
        }
        catch (ApiException ex)
        {
            _logger.LogError("Exception when sending email: " + ex.Message);
            throw new MailSenderException(ex.Message, ex);
        }
    }


    private static byte[] ReadFully(Stream input)
    {
        using MemoryStream ms = new();
        input.CopyTo(ms);
        return ms.ToArray();
    }
}