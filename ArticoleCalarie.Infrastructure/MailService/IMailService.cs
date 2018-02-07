using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Infrastructure.MailService
{
    public interface IMailService
    {
        Task SendMail(EmailModel emailModel);
    }
}
