﻿using System;
using System.Collections.Generic;
using ArticoleCalarie.Models.Enums;

namespace ArticoleCalarie.Models
{
    public class OrderViewModel
    {
        public int OrderNumber { get; set; }

        public string Email { get; set; }

        public bool UserIsLoggedIn { get; set; }

        public List<ShoppingCartItem> ShoppingItems { get; set; }

        public AddressViewModel DeliveryAddress { get; set; }

        public AddressViewModel BillingAddress { get; set; }

        public decimal TotalAmount { get; set; }

        public OrderStatusViewEnum OrderStatus { get; set; }

        public DateTime OrderRegistrationDate { get; set; }

        public decimal DeliveryCost { get; set; }
    }
}
