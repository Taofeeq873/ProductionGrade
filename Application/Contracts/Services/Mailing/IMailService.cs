namespace Application.Contracts.Services.Mailing;

public interface IMailService
{
    Task<bool> SendMail(string email, string name, string subject, string content);

}