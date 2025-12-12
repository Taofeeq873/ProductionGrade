using System.Text;
using System.Text.Json;
using Application.Contracts.Services.Mailing;
using Domain.Configurations;
using Domain.Dtos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

public class EmailConsumerService : BackgroundService
{
    private readonly MessageBrokerConfiguration _config;
    private readonly ILogger<EmailConsumerService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public EmailConsumerService(
        IOptions<MessageBrokerConfiguration> config,
        IServiceScopeFactory scopeFactory,
        ILogger<EmailConsumerService> logger)
    {
        _config = config.Value;
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        return Task.Run(() =>
        {
            var factory = new ConnectionFactory
            {
                HostName = _config.HostName,
                Port = _config.Port,
                UserName = _config.UserName,
                Password = _config.Password
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare("emailQueue",
                true,
                false,
                false,
                null);

            channel.BasicQos(0, 1, false);

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var emailJson = Encoding.UTF8.GetString(body);

                try
                {
                    var messageId = ea.BasicProperties.MessageId;

                    if (!string.IsNullOrEmpty(messageId))
                        _logger.LogInformation($"Received email message with MessageId: {messageId}");

                    using var scope = _scopeFactory.CreateScope();
                    var mailProvider = scope.ServiceProvider.GetRequiredService<IMailProvider>();

                    var email = JsonSerializer.Deserialize<EmailMessage>(emailJson);
                    if (email != null)
                    {
                        await mailProvider.Send(
                            email.From,
                            email.FromName,
                            email.To,
                            email.ToName,
                            email.TemplateId,
                            email.TemplateParams,
                            subject: email.Subject,
                            content: email.Content,
                            attachments: email.Attachments
                        );


                        channel.BasicAck(ea.DeliveryTag, false);

                        _logger.LogInformation($"Email message with MessageId: {messageId} processed successfully.");
                    }
                    else
                    {
                        _logger.LogWarning($"Failed to deserialize email message. MessageId: {messageId}");

                        channel.BasicAck(ea.DeliveryTag, false);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing email message");

                    channel.BasicNack(ea.DeliveryTag, false, false);
                }
            };

            channel.BasicConsume("emailQueue", false, consumer);

            _logger.LogInformation("Email Consumer is running.");

            while (!stoppingToken.IsCancellationRequested) Thread.Sleep(1000);
        }, stoppingToken);
    }
}