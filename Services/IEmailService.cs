
namespace LostAndFoundWeb.Services;

public interface IEmailService
{
    Task SendEmailAsync(string message, string To, string subject);
}