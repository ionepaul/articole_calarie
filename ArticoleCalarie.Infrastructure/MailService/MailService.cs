using System.Net.Mail;
using System.Threading.Tasks;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Infrastructure.MailService
{
    public class MailService : IMailService
    {
        public async Task SendMail(EmailModel emailModel)
        {
            var message = new MailMessage();

            message.From = new MailAddress(emailModel.From);
            message.Subject = emailModel.Subject;
            message.IsBodyHtml = true;
            message.Body = emailModel.Body;

            foreach(var email in emailModel.To)
            {
                message.To.Add(email);
            }

            if (!string.IsNullOrEmpty(emailModel.ReplyTo))
            {
                message.ReplyToList.Add(emailModel.ReplyTo);
            }

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.EnableSsl = true;

                smtpClient.SendCompleted += (s, e) =>
                {
                    smtpClient.Dispose();
                    message.Dispose();
                };

                await smtpClient.SendMailAsync(message);
            }
        }
    }
}