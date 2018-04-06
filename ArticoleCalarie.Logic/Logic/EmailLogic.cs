using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using ArticoleCalarie.Infrastructure;
using ArticoleCalarie.Infrastructure.MailService;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.IRepository;

namespace ArticoleCalarie.Logic.Logic
{
    public class EmailLogic : IEmailLogic
    {
        private IMailService _iMailService;
        private IProductRepository _iProductRepository;

        public EmailLogic(IMailService iMailService, IProductRepository iProductRepository)
        {
            _iMailService = iMailService;
            _iProductRepository = iProductRepository;
        }

        public async Task SendNewOrderNotification(int orderNumber)
        {
            var adminEmails = ConfigurationManager.AppSettings["ArticoleCalarieAdminEmails"];

            var sendTo = adminEmails.Split(';').ToList();

            var template = GetEmailTemplate(MailTemplates.NewOrder);

            var emailModel = new EmailModel
            {
                To = sendTo,
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.NewOrder, orderNumber),
                Body = template
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendWelcomeEmail(string email, string fullName)
        {
            var latestDeals = await _iProductRepository.GetTheLatestTwoProductsForWelcomeEmail();

            var template = BuildWelcomeEmailTemplate(fullName, latestDeals);

            var emailModel = new EmailModel
            {
                To = new List<string> { email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.WelcomeEmail,
                Body = template
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendResetEmail(string email, string callbackUrl)
        {
            var template = GetEmailTemplate(MailTemplates.ResetPassword);

            var dictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.ResetPasswordUrl.ToString().ToLower()] = callbackUrl
            };

            template = MapTemplateDetails(template, dictionary);

            var emailModel = new EmailModel
            {
                To = new List<string> { email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.ResetPasswrod,
                Body = template
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendConfirmationOrderEmail(Order order)
        {
            var template = BuildOrderTemplate(order, MailTemplates.OrderConfirmation);

            var emailModel = new EmailModel
            {
                To = new List<string> { order.Email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.OrderConfirmed, order.OrderNumber),
                Body = template
            };

            await _iMailService.SendMail(emailModel);
        }

        public async Task SendShippedOrderEmail(Order order, string deliveryTime)
        {
            var template = BuildOrderTemplate(order, MailTemplates.OrderShipped, deliveryTime);

            var emailModel = new EmailModel
            {
                To = new List<string> { order.Email },
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = string.Format(MailSubjects.OrderShipped, order.OrderNumber),
                Body = template
            };

            await _iMailService.SendMail(emailModel);
        }

        #region Private Methods

        private string MapTemplateDetails(string template, Dictionary<string, string> dictionary)
        {
            foreach (KeyValuePair<string, string> entry in dictionary)
            {
                template = template.Replace($"{{{entry.Key}}}", entry.Value);
            }

            return template;
        }

        private string GetEmailTemplate(string templatePath)
        {
            var template = File.ReadAllText(HttpContext.Current.Server.MapPath(templatePath));

            return template;
        }

        private string BuildOrderTemplate(Order order, string templateName, string deliveryTime = "")
        {
            var template = GetEmailTemplate(templateName);

            var orderParametersDictionary = order.ToOrderParametersDictionary();

            template = MapTemplateDetails(template, orderParametersDictionary);

            string orderItemPart = string.Empty;

            foreach (var orderItem in order.OrderItems)
            {
                var orderItemTemplate = GetEmailTemplate(MailTemplates.OrderItem);

                var orderItemParametersDictionary = orderItem.ToOrderItemParametersDictionary();

                orderItemPart += MapTemplateDetails(orderItemTemplate, orderItemParametersDictionary);
            }

            var orderDictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.OrderItemsPart.ToString().ToLower()] = orderItemPart,
                [EmailParametersEnum.EstimatedOrderTime.ToString().ToLower()] = deliveryTime
            };

            template = MapTemplateDetails(template, orderDictionary);

            return template;
        }

        private string BuildWelcomeEmailTemplate(string fullName, IEnumerable<Product> products)
        {
            var template = GetEmailTemplate(MailTemplates.WelcomeEmail);

            string productDealsPart = string.Empty;

            foreach(var product in products)
            {
                var productDealTemplate = GetEmailTemplate(MailTemplates.ProductDealsPart);

                var productParametersDictionary = product.ToProductParametersDictionary();

                productDealsPart += MapTemplateDetails(productDealTemplate, productParametersDictionary);
            }

            var welcomeOrderDictionary = new Dictionary<string, string>
            {
                [EmailParametersEnum.Fullname.ToString().ToLower()] = fullName,
                [EmailParametersEnum.ProductDealsPart.ToString().ToLower()] = productDealsPart
            };

            template = MapTemplateDetails(template, welcomeOrderDictionary);

            return template;
        }

        #endregion
    }
}
