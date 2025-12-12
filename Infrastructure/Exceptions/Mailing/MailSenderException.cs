namespace Infrastructure.Exceptions.Mailing;

public class MailSenderException : Exception
{
    public MailSenderException(string message)
        : base(message)
    {
    }

    public MailSenderException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    public string Message { get; set; }
}