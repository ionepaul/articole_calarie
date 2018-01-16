using System;
using System.Configuration;
using System.Threading.Tasks;
using ArticoleCalarie.Infrastructure.MailService;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Constants;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Enums;
using ArticoleCalarie.Repository.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ArticoleCalarie.Logic.Logic
{
    public class AccountLogic : IAccountLogic
    {
        private IAccountRepository _iAccountRepository;
        private IMailService _iMailService;

        public AccountLogic(IAccountRepository iAccountRepository, IMailService iMailService)
        {
            _iAccountRepository = iAccountRepository;
            _iMailService = iMailService;
        }

        #region Public Methods

        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
        {
            var userModel = model.ToUserModel();

            var result = await _iAccountRepository.Register(userModel, model.Password);

            if (result.Succeeded)
            {
                await SendWelcomeEmail(model.Email, model.FullName);
            }

            return result;
        }

        public async Task SendResetPasswordEmail(string email, string callbackUrl)
        {
            string code = await _iAccountRepository.GeneratePasswordResetToken(email);

            if (code == null)
            {
                return;
            }

            callbackUrl += "?code=" + code;

            await SendRestEmail(email, callbackUrl);
        }

        public async Task<SignInStatus> SignIn(LoginViewModel model)
        {
            return await _iAccountRepository.SignIn(model.Email, model.Password, model.RememberMe);
        }

        public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo)
        {
            return await _iAccountRepository.ExternalSignInAsync(loginInfo);
        }

        public async Task<IdentityResult> ResetPassword(ResetPasswordViewModel model)
        {
            var code = model.Code.Replace(" ", "+");

            return await _iAccountRepository.ResetPassword(model.Email, code, model.Password);
        }

        public async Task<IdentityResult> CreateExternalAccountLogin(string email, string fullName, UserLoginInfo loginInfo)
        {
            var userModel = new UserModel { UserName = email, Email = email, FullName = fullName };

            var result = await _iAccountRepository.CreateExternalAccountAndSignIn(userModel, loginInfo);

            if (result.Succeeded)
            {
                await SendWelcomeEmail(email, fullName);
            }

            return result;
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            var userModel = await _iAccountRepository.FindUserByIdAsync(userId);

            if (userModel == null)
            {
                //throw Exception();
            }

            var userViewModel = userModel.ToViewModel();

            return userViewModel;
        }

        public async Task SaveUserAddress(AddressViewModel addressViewModel, string userId)
        {
            var userModel = await _iAccountRepository.FindUserByIdAsync(userId);

            if (userModel == null)
            {
                //throw Exception();
            }

            var address = addressViewModel.ToDbAddress();

            switch (address.AddressType)
            {
                case AddressType.Billing:
                    if (userModel.BillingAddress != null)
                    {
                        userModel.BillingAddress.FullName = address.FullName;
                        userModel.BillingAddress.PhoneNumber = address.PhoneNumber;
                        userModel.BillingAddress.AddressLine1 = address.AddressLine1;
                        userModel.BillingAddress.AddressLine2 = address.AddressLine2;
                        userModel.BillingAddress.PostalCode = address.PostalCode;
                        userModel.BillingAddress.City = address.City;
                        userModel.BillingAddress.County = address.County;
                        userModel.BillingAddress.Country = address.Country;
                    }
                    else
                    {
                        userModel.BillingAddress = address;
                    }
                    break;
                case AddressType.Delivery:
                    if (userModel.DeliveryAddress != null)
                    {
                        userModel.DeliveryAddress.FullName = address.FullName;
                        userModel.DeliveryAddress.PhoneNumber = address.PhoneNumber;
                        userModel.DeliveryAddress.AddressLine1 = address.AddressLine1;
                        userModel.DeliveryAddress.AddressLine2 = address.AddressLine2;
                        userModel.DeliveryAddress.PostalCode = address.PostalCode;
                        userModel.DeliveryAddress.City = address.City;
                        userModel.DeliveryAddress.County = address.County;
                        userModel.DeliveryAddress.Country = address.Country;
                    }
                    else
                    {
                        userModel.DeliveryAddress = address;
                    }
                    break;
                default:
                    //Throw exception invalid address type
                    break;
            }

            try
            {
                await _iAccountRepository.UpdateUserAsync(userModel);
            }
            catch(Exception ex)
            {
                //log and handle
            }
        }

        #endregion 

        #region Private Methods

        private async Task SendWelcomeEmail(string email, string fullName)
        {
            //var templatePath = MailTemplates.WelcomeEmail;

            //var template = File.ReadAllText(HttpContext.Current.Server.MapPath(templatePath));
            var body = "<b>salut" + fullName + "</b>";

            var emailModel = new EmailModel
            {
                To = email,
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.WelcomeEmail,
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        private async Task SendRestEmail(string email, string callbackUrl)
        {
            //var templatePath = MailTemplates.ResetPassword;

            //var template = File.ReadAllText(HttpContext.Current.Server.MapPath(templatePath));
            var body = "Please reset your password by clicking <a href=\"" + callbackUrl + "\">here</a>";

            var emailModel = new EmailModel
            {
                To = email,
                From = ConfigurationManager.AppSettings["ArticoleCalarieEmail"],
                Subject = MailSubjects.ResetPasswrod,
                Body = body
            };

            await _iMailService.SendMail(emailModel);
        }

        #endregion
    }
}
