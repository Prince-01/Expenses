using System.Threading.Tasks;

namespace Expenses.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
