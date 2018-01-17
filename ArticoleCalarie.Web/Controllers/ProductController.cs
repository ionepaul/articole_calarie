using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;

namespace ArticoleCalarie.Web.Controllers
{
    public class ProductController : Controller
    {
        private IProductLogic _iProductLogic;

        public ProductController(IProductLogic iProductLogic)
        {
            _iProductLogic = iProductLogic;
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Add(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }

            //catch exceptions log
            _iProductLogic.AddProduct(productViewModel);

            return View();
        }
    }
}