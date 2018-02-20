using System.Data.Entity;
using System.Threading.Tasks;
using ArticoleCalarie.Repository.Constants;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Identity;
using ArticoleCalarie.Repository.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security.DataProtection;

namespace ArticoleCalarie.Repository.Repository
{
    public class AccountRepository : AbstractRepository<UserModel>, IAccountRepository
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public AccountRepository(ArticoleCalarieDataContext dataContext, ApplicationUserManager userManager, ApplicationSignInManager signInManager) : base(dataContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region Public Methods

        public async Task<SignInStatus> SignIn(string email, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, shouldLockout: false);
        }

        public async Task<IdentityResult> Register(UserModel userModel, string password)
        {
            var result = await _userManager.CreateAsync(userModel, password);

            if (result.Succeeded)
            {
                var currentUser = _userManager.FindByName(userModel.UserName);

                var roleresult = _userManager.AddToRole(currentUser.Id, RepositoryConstants.Role_User);

                await _signInManager.SignInAsync(userModel, isPersistent: false, rememberBrowser: false);
            }

            return result;
        }

        public async Task<string> GeneratePasswordResetToken(string email)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return null;
            }

            var provider = new DpapiDataProtectionProvider("ArticoleCalarie");

            _userManager.UserTokenProvider = new DataProtectorTokenProvider<UserModel>(provider.Create("ResetPassword"));

            string code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);

            return code;
        }

        public async Task<IdentityResult> ResetPassword(string email, string code, string password)
        {
            var user = await _userManager.FindByNameAsync(email);

            if (user == null)
            {
                return null;
            }

            var provider = new DpapiDataProtectionProvider("ArticoleCalarie");

            _userManager.UserTokenProvider = new DataProtectorTokenProvider<UserModel>(provider.Create("ResetPassword"));

            var result = await _userManager.ResetPasswordAsync(user.Id, code, password);

            return result;
        }

        public async Task<SignInStatus> ExternalSignInAsync(ExternalLoginInfo loginInfo)
        {
            return await _signInManager.ExternalSignInAsync(loginInfo, isPersistent: false);
        }

        public async Task<IdentityResult> CreateExternalAccountAndSignIn(UserModel user, UserLoginInfo loginInfo)
        {
            var result = await _userManager.CreateAsync(user);

            if (result.Succeeded)
            {
                result = await _userManager.AddLoginAsync(user.Id, loginInfo);

                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
            }

            return result;
        }

        public async Task<UserModel> FindUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            
            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task<UserModel> GetUserFullUserInfo(string userId)
        {
            var user = await _ctx.Users.Include(x => x.DeliveryAddress).Include(x => x.BillingAddress).FirstOrDefaultAsync(x => string.Equals(x.Id, userId));

            if (user == null)
            {
                return null;
            }

            return user;
        }

        public async Task UpdateUserAsync(UserModel userModel)
        {
            await _userManager.UpdateAsync(userModel);
        }

        #endregion

        #region Protected Methods

        protected void Dispose()
        {
            if (_userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            if (_signInManager != null)
            {
                _signInManager.Dispose();
                _signInManager = null;
            }
        }

        #endregion
    }
}
