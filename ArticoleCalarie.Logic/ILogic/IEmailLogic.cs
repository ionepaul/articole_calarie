using System.Threading.Tasks;
using ArticoleCalarie.Models;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IEmailLogic
    {
        Task SendNewOrderNotification(int orderNumber);
        Task SendWelcomeEmail(string email, string fullName);
        Task SendResetEmail(string email, string callbackUrl);
        Task SendConfirmationOrderEmail(Order order);
        Task SendShippedOrderEmail(Order order, string deliveryTime);
        Task SendContactFromEmail(ContactPageModel contactModel);
    }
}
