namespace Application.Contracts.Services.Queue;

public interface IProducer
{
    void SendToQueue(string jsonData, string queueName);
}