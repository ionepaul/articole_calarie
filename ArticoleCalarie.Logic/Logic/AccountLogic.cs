using System;
using System.Threading.Tasks;
using ArticoleCalarie.Logic.Converters;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
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
        private IEmailLogic _iEmailLogic;

        public AccountLogic(IAccountRepository iAccountRepository, IEmailLogic iEmailLogic)
        {
            _iAccountRepository = iAccountRepository;
            _iEmailLogic = iEmailLogic;
        }

        #region Public Methods

        public async Task<IdentityResult> RegisterUser(RegisterViewModel model)
        {
            var userModel = model.ToUserModel();

            var result = await _iAccountRepository.Register(userModel, model.Password);

            if (result.Succeeded)
            {
                await _iEmailLogic.SendWelcomeEmail(model.Email, model.FullName);
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

            await _iEmailLogic.SendResetEmail(email, callbackUrl);
        }

        public async Task DeleteAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                throw new Exception("Invalid User Identifier");
            }

            await _iAccountRepository.DeleteUser(userId);
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

        public async Task<IdentityResult> CreateExternalAccountLogin(ExternalLoginConfirmationViewModel model, UserLoginInfo loginInfo)
        {
            var userModel = new UserModel
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName,
                IsTermsAccepted = model.IsTermsAccepted,
                IsPrivacyPolicyAccepted = model.IsPrivacyPolicyAccepted,
                IsNewsletterSubscription = model.IsNewsletterSubscription
            };

            var result = await _iAccountRepository.CreateExternalAccountAndSignIn(userModel, loginInfo);

            if (result.Succeeded)
            {
                await _iEmailLogic.SendWelcomeEmail(model.Email, model.FullName);
            }

            return result;
        }

        public async Task<UserViewModel> GetUserById(string userId)
        {
            var userModel = await _iAccountRepository.GetUserFullUserInfo(userId);

            if (userModel == null)
            {
                throw new Exception("Invalid user identifier.");
            }

            var userViewModel = userModel.ToViewModel();

            return userViewModel;
        }

        public async Task SaveUserAddress(AddressViewModel addressViewModel, string userId)
        {
            var userModel = await _iAccountRepository.GetUserFullUserInfo(userId);

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

        public async Task UpdateNewsletterSubscription(string userId, bool isSubscribed)
        {
            var user = await _iAccountRepository.GetUserFullUserInfo(userId);

            if (user == null)
            {
                return;
            }

            user.IsNewsletterSubscription = isSubscribed;

            await _iAccountRepository.UpdateUserAsync(user);
        }

        #endregion 

        #region Private Methods

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
