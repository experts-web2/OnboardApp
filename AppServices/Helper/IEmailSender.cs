using System.Threading.Tasks;

namespace OnboardingApp.Infrastructure.EmailService
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);
    }
}
