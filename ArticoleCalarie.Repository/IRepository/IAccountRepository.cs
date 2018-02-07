using System.Threading.Tasks;
using ArticoleCalarie.Repository.Entities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

namespace ArticoleCalarie.Repository.IRepository
{
    public interface IAccountRepository
    {
        Task<SignInStatus> SignIn(string email, string password, bool rememberMe);
        Task<IdentityResult> Register(UserModel userModel, string password);
        Task<string> GeneratePasswordResetToken(string email);
        Task<IdentityResult> ResetPassword(string email, string code, string password);
        Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo);
        Task<IdentityResult> CreateExternalAccountAndSignIn(UserModel user, UserLoginInfo loginInfo);
        Task<UserModel> FindUserByIdAsync(string userId);
        Task UpdateUserAsync(UserModel userModel);
    }
}
