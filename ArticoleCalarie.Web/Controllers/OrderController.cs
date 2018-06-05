using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using ArticoleCalarie.Logic.ILogic;
using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using NLog;
using PagedList;

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
        [Route("comanda/produse", Name = "shopping-cart-details-url")]
        public ActionResult ShoppingCartDetails()
        {
            _logger.Info("VIEW > Shopping Cart Details");

            return View();
        }

        [HttpGet]
        [Route("comanda/checkout", Name = "checkout-url")]
        public async Task<ActionResult> Checkout()
        {
            _logger.Info("VIEW > Check out");

            try
            {
                var shoppingCart = Session["ShoppingCart"] as ShoppingCartModel;
                var freeDeliveryCostValue = Convert.ToDecimal(ConfigurationManager.AppSettings["FreeDeliveryOrderValue"]);
                var deliveryCost = Convert.ToDecimal(ConfigurationManager.AppSettings["DeliveryCost"]);

                var orderViewModel = new OrderViewModel()
                {
                    ShoppingItems = shoppingCart.ShoppingItems,
                    DeliveryCost = shoppingCart.ShoppingItems.Sum(x => x.Price) >= freeDeliveryCostValue ? 0 : deliveryCost
                };
                
                if (User.Identity.IsAuthenticated)
                {
                    _logger.Info("User is logged in.");

                    var userId = User.Identity.GetUserId();

                    var userViewModel = await _iAccountLogic.GetUserById(userId);

                    _logger.Info("Successfully retrieved user information.");

                    orderViewModel.Email = userViewModel.Email;
                    orderViewModel.DeliveryAddress = userViewModel.DeliveryAddress;
                    orderViewModel.BillingAddress = userViewModel.BillingAddress;
                    orderViewModel.UserIsLoggedIn = true;

                    Session["OrderModel"] = orderViewModel;

                    return View(orderViewModel);
                }

                _logger.Info("User is not logged in.");

                orderViewModel.UserIsLoggedIn = false;

                Session["OrderModel"] = orderViewModel;

                return View(orderViewModel);
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
            var orderViewModel = Session["OrderModel"] as OrderViewModel;

            orderViewModel.Email = guestEmailModel.Email;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task UpdateDeliveryAddress(AddressViewModel address)
        {
            _logger.Info("POST > Saving Delivery Address for checkout.");

            try
            {
                address.AddressType = AddressTypeViewEnum.Delivery;

                var orderViewModel = Session["OrderModel"] as OrderViewModel;
                 
                var userId = User.Identity.GetUserId();

                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.Info("User is logged in, updating delivery address on profile.");

                    await _iAccountLogic.SaveUserAddress(address, userId);
                }

                orderViewModel.DeliveryAddress = address;
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
                AddressViewModel billingAddress = new AddressViewModel();

                var orderViewModel = Session["OrderModel"] as OrderViewModel;

                var userId = User.Identity.GetUserId();

                if (address.IsSameAsDelivery)
                {
                    billingAddress = new AddressViewModel()
                    {
                        AddressLine1 = orderViewModel?.DeliveryAddress?.AddressLine1,
                        AddressLine2 = orderViewModel?.DeliveryAddress?.AddressLine2,
                        City = orderViewModel?.DeliveryAddress?.City,
                        Country = orderViewModel?.DeliveryAddress?.Country,
                        County = orderViewModel?.DeliveryAddress?.County,
                        PostalCode = orderViewModel?.DeliveryAddress?.PostalCode,
                        PhoneNumber = orderViewModel?.DeliveryAddress?.PhoneNumber,
                        FullName = orderViewModel?.DeliveryAddress?.FullName
                    };
                }
                else
                {
                    billingAddress = new AddressViewModel()
                    {
                        AddressLine1 = address.AddressLine1,
                        AddressLine2 = address.AddressLine2,
                        City = address.City,
                        Country = address.Country,
                        County = address.County,
                        PostalCode = address.PostalCode,
                        PhoneNumber = address.PhoneNumber,
                        FullName = address.FullName
                    };
                }

                billingAddress.AddressType = AddressTypeViewEnum.Billing;

                if (!string.IsNullOrEmpty(userId))
                {
                    _logger.Info("User is logged in, updating billing address on profile.");

                    await _iAccountLogic.SaveUserAddress(billingAddress, userId);
                }

                orderViewModel.BillingAddress = billingAddress;
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
                var orderViewModel = Session["OrderModel"] as OrderViewModel;

                orderViewModel.TotalAmount = totalAmount;

                var userId = User.Identity.GetUserId();

                _logger.Info($"Saving order...Object: {JsonConvert.SerializeObject(orderViewModel)}");

                var orderNumber = await _iOrderLogic.PlaceOrder(orderViewModel, userId);

                Session["OrderModel"] = null;
                Session["ShoppingCart"] = null;

                _logger.Info("Successfully saved order");

                return RedirectToAction(nameof(CheckoutDone), new { nrComanda = orderNumber });
            }
            catch (Exception ex)
            {
                _logger.Info($"Failed to save order. Exception: {ex.Message}");

                return View("Error");
            }
        }

        [HttpGet]
        [Route("comanda/comanda-inregistrata", Name = "checkout-done-url")]
        public ActionResult CheckoutDone(int nrComanda)
        {
            _logger.Info("VIEW > Checkout Done");

            return View(nrComanda);
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> OrderList(int? pageNumber, OrderStatusViewEnum status = OrderStatusViewEnum.REGISTRED)
        {
            _logger.Info("VIEW > Admin > All orders");

            ViewBag.OrderStatus = status;

            try
            {
                var page = pageNumber ?? 1;

                _logger.Info($"Getting orders for admin, page {page}, status {status.ToString()}.");

                var ordersModel = await _iOrderLogic.GetOrders(page, status);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["AdminOrdersPerPage"]);

                var pagedListModel = new StaticPagedList<OrderViewModel>(ordersModel.Orders, page, pageSize, ordersModel.TotalCount);

                _logger.Info("Successfully got orders list for admin.");

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to retrive orders for status {status.ToString()}. Exception: {ex.Message}");

                return View("Error");
            }
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> OrderListPartial(int? pageNumber, OrderStatusViewEnum status = OrderStatusViewEnum.ALL)
        {
            _logger.Info("PARTIAL VIEW > Orders List");

            ViewBag.OrderStatus = status;

            try
            {
                var page = pageNumber ?? 1;

                _logger.Info($"Getting orders for admin, page {page}, status {status.ToString()}.");

                var ordersModel = await _iOrderLogic.GetOrders(page, status);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["AdminOrdersPerPage"]);

                var pagedListModel = new StaticPagedList<OrderViewModel>(ordersModel.Orders, page, pageSize, ordersModel.TotalCount);

                _logger.Info("Successfully got orders list for admin.");

                return PartialView("_OrderListPartial", pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to retrive orders for status {status.ToString()}. Exception: {ex.Message}");

                return View("Error");
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task ChangeOrderStatus(ChangeOrderStatusModel changeStatusOrderModel)
        {
            _logger.Info("POST > Change order status");

            try
            {
                await _iOrderLogic.ChangeOrderStatus(changeStatusOrderModel.OrderNumber, changeStatusOrderModel.NewOrderStatus, changeStatusOrderModel.DeliveryTime);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to change order #{changeStatusOrderModel.OrderNumber} status to {changeStatusOrderModel.NewOrderStatus}. Exception: {ex.Message}.");

                throw ex;
            }
        }

        [HttpGet]
        [Authorize]
        [Route("account/comenzile-mele", Name = "user-orders-url")]
        public async Task<ActionResult> UserOrderList(int? pageNumber)
        {
            _logger.Info("VIEW > User order list");

            try
            {
                var userId = User.Identity.GetUserId();

                var page = pageNumber ?? 1;

                _logger.Info($"Getting user order list, page {page}.");

                var ordersModel = await _iOrderLogic.GetUserOrders(page, userId);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["UserOrdersPerPage"]);

                var pagedListModel = new StaticPagedList<OrderViewModel>(ordersModel.Orders, page, pageSize, ordersModel.TotalCount);

                _logger.Info("Successfully got user order list");

                return View(pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get user list. Exception: {ex.Message}");

                return View("Error");
            }
        }

        [HttpGet]
        [Authorize]
        [Route("account/comenzile-mele-partial", Name = "user-orders-partial-url")]
        public async Task<ActionResult> UserOrderListPartial(int? pageNumber)
        {
            _logger.Info("VIEW > User order list");

            try
            {
                var userId = User.Identity.GetUserId();

                var page = pageNumber ?? 1;

                _logger.Info($"Getting user order list, page {page}.");

                var ordersModel = await _iOrderLogic.GetUserOrders(page, userId);

                int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["UserOrdersPerPage"]);

                var pagedListModel = new StaticPagedList<OrderViewModel>(ordersModel.Orders, page, pageSize, ordersModel.TotalCount);

                _logger.Info("Successfully got user order list");

                return PartialView("_UserOrderListPartial", pagedListModel);
            }
            catch (Exception ex)
            {
                _logger.Error($"Failed to get user list. Exception: {ex.Message}");

                return View("Error");
            }
        }
    }
}