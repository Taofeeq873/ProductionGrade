using System.Text;
using Application.Contracts.Services.Queue;
using Domain.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;

namespace Infrastructure.Services.Queue;

public class Producer : IProducer
{
    private readonly MessageBrokerConfiguration _config;
    private readonly ILogger<Producer> _logger;

    public Producer(IOptions<MessageBrokerConfiguration> config, ILogger<Producer> logger)
    {
        _config = config.Value;
        _logger = logger;
    }
    public void SendToQueue(string jsonData, string queueName)
    {
        try
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

            channel.QueueDeclare(queueName,
                true,
                false,
                false,
                null);

            var body = Encoding.UTF8.GetBytes(jsonData);

            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;
            properties.MessageId = Guid.NewGuid().ToString();
            channel.BasicPublish("",
                queueName,
                properties,
                body);

            _logger.LogInformation("Message published to queue '{QueueName}'", queueName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error publishing message to queue '{QueueName}'", queueName);
        }
    }
}