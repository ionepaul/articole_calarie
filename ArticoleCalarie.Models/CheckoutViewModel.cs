using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class CheckoutViewModel
    {
        [Required]
        public string Email { get; set; }

        public bool UserIsLoggedIn { get; set; }

        public List<ShoppingCartItem> ShoppingItems { get; set; }

        public AddressViewModel DeliveryAddress { get; set; }

        public AddressViewModel BillingAddress { get; set; }
    }
}
