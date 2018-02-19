using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using Microsoft.AspNet.Identity;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class OrderController : Controller
    {
        private static Logger _logger;
        private IAccountLogic _iAccountLogic;

        public OrderController(IAccountLogic iAccountLogic)
        {
            _logger = LogManager.GetLogger("Order");
            _iAccountLogic = iAccountLogic;
        }

        [HttpGet]
        public ActionResult ShoppingCartDetails()
        {
            _logger.Info("VIEW > Shopping Cart Details");

            return View();
        }

        [HttpGet]
        public async Task<ActionResult> Checkout()
        {
            _logger.Info("VIEW > Check out");

            try
            {
                if (User.Identity.IsAuthenticated)
                {
                    _logger.Info("User is logged in.");

                    var userId = User.Identity.GetUserId();

                    var userViewModel = await _iAccountLogic.GetUserById(userId);

                    _logger.Info("Successfully retrieved user information.");

                    return View(userViewModel);
                }

                _logger.Info("User is not logged in.");

                return View();
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to go to checkout page. Exception {ex.Message}");

                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult AddToCart(ShoppingCartItem shoppingCartItem)
        {
            _logger.Info("POST > Add to cart");

            try
            {
                var sessionShoppingCart = Session["ShoppingCart"] as ShoppingCartModel;

                if (sessionShoppingCart == null)
                {
                    var shoppingCartModel = new ShoppingCartModel
                    {
                        ShoppingItems = new List<ShoppingCartItem> { shoppingCartItem }
                    };

                    Session["ShoppingCart"] = shoppingCartModel;
                }
                else
                {
                    sessionShoppingCart.ShoppingItems.Add(shoppingCartItem);
                }

                return PartialView("_ShoppingCartPartial");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to add product to shopping cart, product {shoppingCartItem.ProductName}. Exception {ex.Message}");

                return View("Error");
            }
        }

        [HttpPost]
        public ActionResult DeleteFromCart(string productCode)
        {
            _logger.Info("POST > Delete from cart");

            try
            {
                var sessionShoppingCart = Session["ShoppingCart"] as ShoppingCartModel;

                sessionShoppingCart.ShoppingItems.RemoveAll(x => string.Equals(x.ProductCode, productCode, StringComparison.CurrentCultureIgnoreCase));

                return PartialView("_ShoppingCartPartial");
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to delete product from shopping cart, product code {productCode}. Exception {ex.Message}");

                return View("Error");
            }
        }

        [HttpPost]
        public async Task<ActionResult> UpdateDeliveryAddress(AddressViewModel address)
        {
            if (!ModelState.IsValid)
            {
                return View("Checkout");
            }

            var userId = User.Identity.GetUserId();

            address.AddressType = AddressTypeViewEnum.Delivery;

            await _iAccountLogic.SaveUserAddress(address, userId);

            return View();
        }
    }
}