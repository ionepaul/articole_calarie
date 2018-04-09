using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class HomeController : Controller
    {
        private static Logger _logger;
        private IEmailLogic _iEmailLogic;

        public HomeController(IEmailLogic iEmailLogic)
        {
            _logger = LogManager.GetLogger("Home");
            _iEmailLogic = iEmailLogic;
        }

        public ActionResult Index()
        {
            return View();
        }

        [Route("despre-noi", Name = "about-us-url")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        [HttpGet]
        [Route("contact", Name = "contact-url")]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("contact-form", Name = "contact-form-url")]
        public async Task<ActionResult> Contact(ContactPageModel contactModel)
        {
            _logger.Info("Sending contact form email.");

            if (!ModelState.IsValid)
            {
                _logger.Info("Contact model invalid, returning contact view.");

                return View(contactModel);
            }

            try
            {
                await _iEmailLogic.SendContactFromEmail(contactModel);

                _logger.Info($"Successfully sent contact form email.");

                return RedirectToRoute("contact-message-sent-url");
            }
            catch(Exception ex)
            {
                _logger.Error($"Something went wrong sending the contact form email. Exception: {ex.Message}.");

                return View("Error");
            }
        }

        [HttpGet]
        [Route("contact/mesaj-trimis", Name = "contact-message-sent-url")]
        public ActionResult ContactMessageSent()
        {
            return View();
        }
    }
}