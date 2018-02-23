using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NLog;

namespace ArticoleCalarie.Web.Controllers
{
    public class OrderController : Controller
    {
        private static Logger _logger;
        private IAccountLogic _iAccountLogic;
        private IOrderLogic _iOrderLogic;

        public OrderController(IAccountLogic iAccountLogic, IOrderLogic iOrderLogic)
        {
            _logger = LogManager.GetLogger("Order");
            _iAccountLogic = iAccountLogic;
            _iOrderLogic = iOrderLogic;
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
                var shoppingCart = Session["ShoppingCart"] as ShoppingCartModel;

                var checkoutViewModel = new CheckoutViewModel()
                {
                    ShoppingItems = shoppingCart.ShoppingItems
                };
                
                if (User.Identity.IsAuthenticated)
                {
                    _logger.Info("User is logged in.");

                    var userId = User.Identity.GetUserId();

                    var userViewModel = await _iAccountLogic.GetUserById(userId);

                    _logger.Info("Successfully retrieved user information.");

                    checkoutViewModel.Email = userViewModel.Email;
                    checkoutViewModel.DeliveryAddress = userViewModel.DeliveryAddress;
                    checkoutViewModel.BillingAddress = userViewModel.BillingAddress;
                    checkoutViewModel.UserIsLoggedIn = true;

                    Session["CheckoutModel"] = checkoutViewModel;

                    return View(checkoutViewModel);
                }

                _logger.Info("User is not logged in.");

                checkoutViewModel.UserIsLoggedIn = false;

                Session["CheckoutModel"] = checkoutViewModel;

                return View(checkoutViewModel);
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
                    if (sessionShoppingCart.ShoppingItems.Any(x => string.Equals(x.ProductCode, shoppingCartItem.ProductCode) && 
                        string.Equals(x.Color, shoppingCartItem.Color) && string.Equals(x.Size, shoppingCartItem.Size)))
                    {
                        sessionShoppingCart.ShoppingItems.First(x => string.Equals(x.ProductCode, shoppingCartItem.ProductCode)).Quantity += shoppingCartItem.Quantity;
                    }
                    else
                    {
                        sessionShoppingCart.ShoppingItems.Add(shoppingCartItem);
                    }
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
        [ValidateAntiForgeryToken]
        public void SaveGuestEmail(GuestEmailViewModel guestEmailModel)
        {
            var checkoutViewModel = Session["CheckoutModel"] as CheckoutViewModel;

            checkoutViewModel.Email = guestEmailModel.Email;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task UpdateDeliveryAddress(AddressViewModel address)
        {
            _logger.Info("POST > Saving Delivery Address for checkout.");

            try
            {
                address.AddressType = AddressTypeViewEnum.Delivery;

                var checkoutViewModel = Session["CheckoutModel"] as CheckoutViewModel;
                 
                var userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.Info("User is logged in, updating delivery address on profile.");

                    await _iAccountLogic.SaveUserAddress(address, userId);
                }

                checkoutViewModel.DeliveryAddress = address;
                checkoutViewModel.DeliveryAddress.AddressType = AddressTypeViewEnum.Delivery;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update delivery address when checkout. Exception: {ex.Message}");

                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task UpdateBillingAddress(AddressViewModel address)
        {
            _logger.Info("POST > Saving billing address for checkout.");

            try
            {
                var checkoutViewModel = Session["CheckoutModel"] as CheckoutViewModel;

                var userId = User.Identity.GetUserId();

                if (address.IsSameAsDelivery)
                {
                    address = checkoutViewModel.DeliveryAddress;
                }

                address.AddressType = AddressTypeViewEnum.Billing;

                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.Info("User is logged in, updating billing address on profile.");

                    await _iAccountLogic.SaveUserAddress(address, userId);
                }

                checkoutViewModel.BillingAddress = address;
                checkoutViewModel.BillingAddress.AddressType = AddressTypeViewEnum.Billing;
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to update billing address when checkout. Exception: {ex.Message}");

                throw ex;
            }
        }

        public async Task<ActionResult> PlaceOrder(decimal totalAmount)
        {
            _logger.Info("POST > Place Order");

            try
            {
                var checkoutViewModel = Session["CheckoutModel"] as CheckoutViewModel;

                checkoutViewModel.TotalAmount = totalAmount;

                var userId = User.Identity.GetUserId();

                _logger.Info($"Saving order...Object: {JsonConvert.SerializeObject(checkoutViewModel)}");

                var orderNumber = await _iOrderLogic.PlaceOrder(checkoutViewModel, userId);

                Session["CheckoutModel"] = null;
                Session["ShoppingCart"] = null;

                _logger.Info("Successfully saved order");

                return RedirectToAction(nameof(CheckoutDone), orderNumber);
            }
            catch (Exception ex)
            {
                _logger.Info($"Failed to save order. Exception: {ex.Message}");

                return View("Error");
            }
        }

        [HttpGet]
        public ActionResult CheckoutDone(int orderNumber)
        {
            _logger.Info("VIEW > Checkout Done");

            return View(orderNumber);
        }
    }
}