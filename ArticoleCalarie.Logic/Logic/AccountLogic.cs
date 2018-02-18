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
               // await SendWelcomeEmail(model.Email, model.FullName);
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

            await SendResetEmail(email, callbackUrl);
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
            var userModel = new UserModel
            {
                UserName = email,
                Email = email,
                FullName = fullName
            };

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
                throw new Exception("Invalid user identifier.");
            }

            var userViewModel = userModel.ToViewModel();

            return userViewModel;
        }

        public async Task SaveUserAddress(AddressViewModel addressViewModel, string userId)
        {
            var userModel = await _iAccountRepository.FindUserByIdAsync(userId);

            if (userModel == null)
            {
                throw new Exception("Invalid user identifier.");
            }

            var address = addressViewModel.ToDbAddress();

            switch (address.AddressType)
            {
                case AddressType.Billing:
                    if (userModel.BillingAddress != null)
                    {
                        UpdateAddress(userModel.BillingAddress, address);
                    }
                    else
                    {
                        userModel.BillingAddress = address;
                    }
                    break;
                case AddressType.Delivery:
                    if (userModel.DeliveryAddress != null)
                    {
                        UpdateAddress(userModel.DeliveryAddress, address);
                    }
                    else
                    {
                        userModel.DeliveryAddress = address;
                    }
                    break;
                default:
                    throw new Exception("Invalid address type.");
            }

            await _iAccountRepository.UpdateUserAsync(userModel);
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

        private async Task SendResetEmail(string email, string callbackUrl)
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

        private void UpdateAddress(Address oldAddress, Address newAddress)
        {
            oldAddress.FullName = newAddress.FullName;
            oldAddress.PhoneNumber = newAddress.PhoneNumber;
            oldAddress.AddressLine1 = newAddress.AddressLine1;
            oldAddress.AddressLine2 = newAddress.AddressLine2;
            oldAddress.PostalCode = newAddress.PostalCode;
            oldAddress.City = newAddress.City;
            oldAddress.County = newAddress.County;
            oldAddress.Country = newAddress.Country;
        }

        #endregion
    }
}
