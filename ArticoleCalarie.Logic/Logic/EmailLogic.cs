using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using ArticoleCalarie.Infrastructure.MailService;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
using ArticoleCalarie.Repository.Entities;

namespace ArticoleCalarie.Logic.Logic
{
    public class EmailLogic : IEmailLogic
    {
        private IMailService _iMailService;

        public EmailLogic(IMailService iMailService)
        {
            _iMailService = iMailService;
        }

        public async Task SendNewOrderNotification(int orderNumber)
        {
            var adminEmails = ConfigurationManager.AppSettings["ArticoleCalarieAdminEmails"];

            var sendTo = adminEmails.Split(';').ToList();

            var body = $"<b>Confirma comanda noua. </b> <br/> <h3>Comanda: #{orderNumber}</h3>";

            var emailModel = new EmailModel
            {
                To = sendTo,
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.NewOrder, orderNumber),
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendWelcomeEmail(string email, string fullName)
        {
            //var templatePath = MailTemplates.WelcomeEmail;

            //var template = File.ReadAllText(HttpContext.Current.Server.MapPath(templatePath));
            var body = $"<b>salut {fullName}</b>";

            var emailModel = new EmailModel
            {
                To = new List<string> { email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.WelcomeEmail,
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendResetEmail(string email, string callbackUrl)
        {
            //var templatePath = MailTemplates.ResetPassword;

            //var template = File.ReadAllText(HttpContext.Current.Server.MapPath(templatePath));
            var body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            var emailModel = new EmailModel
            {
                To = new List<string> { email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.ResetPasswrod,
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendConfirmationOrderEmail(Order order)
        {
            var body = $"<p>Comanda #{order.OrderNumber} a fost confirmata. Detailii....<br /> Veti primi un mail cand va fi livrata.<p>";

            var emailModel = new EmailModel
            {
                To = new List<string> { order.Email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.OrderConfirmed, order.OrderNumber),
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendShippedOrderEmail(Order order)
        {
            var body = $"<p>Comanda #{order.OrderNumber} a fost trimisa. Detailii....<br /> Va veni in x zile.<p>";

            var emailModel = new EmailModel
            {
                To = new List<string> { order.Email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.OrderShipped, order.OrderNumber),
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }
    }
}
