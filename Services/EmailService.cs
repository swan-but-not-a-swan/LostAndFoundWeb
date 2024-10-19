using LostAndFoundWeb.Data;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using MimeKit;

namespace LostAndFoundWeb.Services;

public class EmailService: IEmailService,IEmailSender<ApplicationUser>
{
    private string password;
    public EmailService(IConfiguration configuration)
    {
        password = configuration.GetValue<string>("emailpassword");
    }
    public async Task SendEmailAsync(string message, string To, string subject)
    {
        MimeMessage email = new();
        email.From.Add(MailboxAddress.Parse("notifications@codecain.tech"));
        email.To.Add(MailboxAddress.Parse(To));
        email.Subject = subject;
        email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message };
        using var smtp = new SmtpClient();
        
            await smtp.ConnectAsync("mail.codecain.tech", 465, MailKit.Security.SecureSocketOptions.SslOnConnect);
            smtp.AuthenticationMechanisms.Remove("XOAUTH2");
            await smtp.AuthenticateAsync("notifications@codecain.tech", password);
            await smtp.SendAsync(email);
        
       
    }

    public async Task SendConfirmationLinkAsync(ApplicationUser user, string email, string confirmationLink)
    {
        string message = $"Confirm your email Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.";
        await SendEmailAsync(message, email, "Confirm your email");
    }
    public Task SendPasswordResetLinkAsync(ApplicationUser user, string email, string resetLink) => throw new NotImplementedException();
    public Task SendPasswordResetCodeAsync(ApplicationUser user, string email, string resetCode) => throw new NotImplementedException();
}
