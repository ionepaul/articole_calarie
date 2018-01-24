﻿using System.Threading.Tasks;
using ArticoleCalarie.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ArticoleCalarie.Logic.ILogic
{
    public interface IAccountLogic
    {
        Task<SignInStatus> SignIn(LoginViewModel model);
        Task<IdentityResult> RegisterUser(RegisterViewModel model);
        Task SendResetPasswordEmail(string email, string callbackUrl);
        Task<IdentityResult> ResetPassword(ResetPasswordViewModel model);
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo);
        Task<IdentityResult> CreateExternalAccountLogin(string email, string fullName, UserLoginInfo loginInfo);
        Task<UserViewModel> GetUserById(string userId);
        Task SaveUserAddress(AddressViewModel addressViewModel, string userId);
    }
}