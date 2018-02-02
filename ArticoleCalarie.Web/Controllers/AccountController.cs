using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private static Logger _logger;
        private IAccountLogic _iAccountLogic;

        public AccountController(IAccountLogic iAccountLogic)
        {
            _logger = LogManager.GetLogger("Account");
            _iAccountLogic = iAccountLogic;
        }

        #region Login

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            _logger.Info("VIEW > Login");

            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            _logger.Info("POST > Login");

            if (!ModelState.IsValid)
            {
                _logger.Info("Invalid LoginModel. Returning Login view.");

                return View(model);
            }

            try
            {
                var result = await _iAccountLogic.SignIn(model);

                switch (result)
                {
                    case SignInStatus.Success:
                        _logger.Info("Successfully logged user in.");
                        return RedirectToLocal(returnUrl);
                    default:
                        _logger.Warn($"Failed to log in user: {model.Email}. Result was unsuccessful. Returning Login view.");
                        ModelState.AddModelError("", "Invalid login attempt.");
                        return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to log in user: {model.Email}. Exception: {ex.Message}.");

                return View("Error");
            } 
        }

        #endregion

        #region Register

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Register()
        {
            _logger.Info("VIEW > Register");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            _logger.Info("POST > Register");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _iAccountLogic.RegisterUser(model);

                    if (result.Succeeded)
                    {
                        _logger.Info($"Successfully registred user: {model.Email}.");

                        return RedirectToAction("Index", "Home");
                    }

                    AddErrors(result);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to register user: {model.Email}. Exception: {ex.Message}.");

                    return View("Error");
                }
            }

            _logger.Warn($"Failed to register user: {model.Email}. Result was unsuccessful. Returning register view.");

            return View(model);
        }

        #endregion

        #region Forgot Password

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            _logger.Info("VIEW > Forgot Password");

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            _logger.Info("POST > Forgot Password");

            if (!ModelState.IsValid)
            {
                _logger.Info("Invalid ForgotPasswordModel. Returning Forgot Password View");

                return View(model);
            }

            try
            {
                var callbackUrl = Url.Action("ResetPassword", "Account", new { }, protocol: Request.Url.Scheme);

                await _iAccountLogic.SendResetPasswordEmail(model.Email, callbackUrl);

                _logger.Info($"Successfully sent reset password email to {model.Email}");

                return RedirectToAction("ForgotPasswordConfirmation", "Account");
            }
            catch(Exception ex)
            {
                _logger.Error($"Failed to send reset password email to {model.Email}. Exception {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            _logger.Info("VIEW > Forogt Password Confirmation");

            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            _logger.Info("View > Account > Reset Password");

            return code == null ? View("Error") : View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            _logger.Info("POST > Reset Password");

            if (ModelState.IsValid)
            {
                try
                {
                    var result = await _iAccountLogic.ResetPassword(model);

                    if (result.Succeeded || result == null)
                    {
                        _logger.Info($"Successfully reset password for user: {model.Email}");

                        return RedirectToAction("ResetPasswordConfirmation", "Account");
                    }

                    AddErrors(result);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to reset password for user: {model.Email}. Exception: {ex.Message}");

                    return View("Error");
                }
            }

            _logger.Warn($"Failed to reset password for user: {model.Email}. Result was unsuccessful. Returning Reset Password View.");

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            _logger.Info("VIEW > Reset Password Confirmation");

            return View();
        }

        #endregion

        #region External Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            _logger.Info("POST > External Login");
            _logger.Info("Redirecting user to an external log in provider.");

            return new ChallengeResult(provider, Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            _logger.Info("GET > External Login Callback");

            try
            {
                var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();

                if (loginInfo == null)
                {
                    _logger.Info("Could not find external login info. Returning Login view.");

                    return RedirectToAction("Login");
                }

                // Sign in the user with this external login provider if the user already has a login
                var result = await _iAccountLogic.ExternalSignInAsync(loginInfo);

                switch (result)
                {
                    case SignInStatus.Success:
                        _logger.Info("Successfully logged in external user.");
                        return RedirectToLocal(returnUrl);
                    default:
                        _logger.Info("Prompt the user to confirm registration through external provider.");
                        ViewBag.ReturnUrl = returnUrl;
                        ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                        var fullName = loginInfo.ExternalIdentity.Claims.First(c => c.Type == "urn:facebook:name")?.Value;
                        return RedirectToAction("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { Email = loginInfo.Email, FullName = fullName });
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to logged in user through external provider. Exception {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model)
        {
            _logger.Info("VIEW > External Login Confirmation");

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            _logger.Info("POST > External Login Confirmation");

            if (User.Identity.IsAuthenticated)
            {
                _logger.Info($"User: {model.Email} is already authenticated. Returning Account Manage View");

                return RedirectToAction(nameof(Manage));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Get the information about the user from the external login provider
                    var info = await AuthenticationManager.GetExternalLoginInfoAsync();

                    if (info == null)
                    {
                        _logger.Info("Could not find external login info. Returning ExternalLoginFailure view.");

                        return View("ExternalLoginFailure");
                    }

                    var result = await _iAccountLogic.CreateExternalAccountLogin(model.Email, model.FullName, info.Login);

                    if (result.Succeeded)
                    {
                        _logger.Info($"Successfully created account through external provider for user: {model.Email}.");

                        return RedirectToLocal(returnUrl);
                    }

                    AddErrors(result);
                }
                catch (Exception ex)
                {
                    _logger.Error($"Failed to create account through external provider for user: {model.Email}. Exception {ex.Message}.");
                    
                    return View("Error");
                }
            }

            _logger.Warn($"Failed to create account through external provider for user: {model.Email}. Result was unsuccessful. Returning ExteranlLoginConfirmation view.");

            ViewBag.ReturnUrl = returnUrl;

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult ExternalLoginFailure()
        {
            _logger.Info("VIEW > External Login Failure");

            return View();
        }

        #endregion

        #region Account Management

        [HttpGet]
        public async Task<ActionResult> Manage()
        {
            _logger.Info("VIEW > Manage");

            try
            {
                var userId = User.Identity.GetUserId();

                UserViewModel userViewModel = await _iAccountLogic.GetUserById(userId);

                _logger.Info("Successfully retrieved user information.");

                return View(userViewModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to retrive user information. Exception {ex.Message}");

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveDeliveryAddress(UserViewModel userViewModel)
        {
            _logger.Info("POST > Save Delivery Address");

            try
            {
                var userId = User.Identity.GetUserId();

                userViewModel.DeliveryAddress.AddressType = AddressTypeViewEnum.Delivery;

                await _iAccountLogic.SaveUserAddress(userViewModel.DeliveryAddress, userId);

                _logger.Info($"Successfully saved delivery address for user: {userViewModel.Email}.");

                return RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                _logger.Info($"Failed to save delivery address for user: {userViewModel.Email}. Exception {ex.Message}.");

                return View("Error");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SaveBillingAddress(UserViewModel userViewModel)
        {
            _logger.Info("POST > Save Billing Address");

            try
            {
                var userId = User.Identity.GetUserId();

                userViewModel.BillingAddress.AddressType = AddressTypeViewEnum.Billing;

                await _iAccountLogic.SaveUserAddress(userViewModel.BillingAddress, userId);

                _logger.Info($"Successfully saved billing address for user: {userViewModel.Email}.");

                return RedirectToAction(nameof(Manage));
            }
            catch (Exception ex)
            {
                _logger.Info($"Failed to save billing address for user: {userViewModel.Email}. Exception {ex.Message}.");

                return View("Error");
            }
        }

        #endregion

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            _logger.Info("POST > Log Off");

            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);

            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Helpers

        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };

                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }

                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }

        #endregion
    }
}