using System.Threading.Tasks;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IEmailLogic
    {
        Task SendNewOrderNotification(int orderNumber);
        Task SendWelcomeEmail(string email, string fullName);
        Task SendResetEmail(string email, string callbackUrl);
    }
}
