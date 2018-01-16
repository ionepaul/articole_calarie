namespace ArticoleCalarie.Models
{
    public class UserViewModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public AddressViewModel BillingAddress { get; set; }
        public AddressViewModel DeliveryAddress { get; set; }
    }
}
