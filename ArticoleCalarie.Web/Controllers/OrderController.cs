using System;
using System.Collections.Generic;
using System.Web.Mvc;
using ArticoleCalarie.Models;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class OrderController : Controller
    {
        private static Logger _logger;

        public OrderController()
        {
            _logger = LogManager.GetLogger("Order");
        }

        [HttpGet]
        public ActionResult ShoppingCartDetails()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Checkout()
        {
            return View();
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
    }
}