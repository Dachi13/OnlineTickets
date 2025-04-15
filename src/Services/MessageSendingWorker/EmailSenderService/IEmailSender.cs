namespace MessageSendingWorker.EmailSenderService;

public interface IEmailSender
{
    Task SendEmailAsync(string subject, string toEmail, string username, string message);
}