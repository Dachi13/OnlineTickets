using MessageSendingWorker.Models;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace MessageSendingWorker.EmailSenderService;

public class EmailSender(IOptions<Config> emailConfig, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly EmailConfig _emailConfig = emailConfig.Value.EmailConfig;
    private static readonly string CachedHtml = GetHtmlFromFile();

    public async Task SendEmailAsync(string subject, string toEmail, string username, string message)
    {
        var apiKey = _emailConfig.ApiKey;
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress(_emailConfig.FromEmail, _emailConfig.Name);
        var to = new EmailAddress(toEmail, username);
        var plainTextContent = message;
        var htmlContent = CachedHtml.Replace("[username]", username).Replace("[message]", message);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

        var response = await client.SendEmailAsync(msg);
        
        if(!response.IsSuccessStatusCode)
            logger.LogError($"[EMAIL FAILED] Failed to send email for \n\t{username} : {toEmail}.]");
    }

    private static string GetHtmlFromFile()
    {
        var path = GetPathToEmailHtml();
        using var reader = new StreamReader(path);
        var extractedHtml = reader.ReadToEnd();

        return extractedHtml;
    }

    private static string GetPathToEmailHtml()
    {
        var baseDirectoryPath = Directory.GetParent(Directory.GetCurrentDirectory())!.FullName;
        var parentDirectoryPath = Directory.GetParent(Directory.GetCurrentDirectory())!.Name;
        var workerPath = string.Concat(parentDirectoryPath, ".Worker");
        var path = Path.Combine(baseDirectoryPath, workerPath, "EmailContext", "EmailTemplate.html");

        return path;
    }
}