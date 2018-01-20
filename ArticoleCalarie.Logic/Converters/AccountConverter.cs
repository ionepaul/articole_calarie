using ArticoleCalarie.Models;
using ArticoleCalarie.Models.Enums;
using ArticoleCalarie.Repository.Entities;
using ArticoleCalarie.Repository.Enums;

namespace ArticoleCalarie.Logic.Converters
{
    public static class AccountConverter
    {
        public static UserModel ToUserModel(this RegisterViewModel registerModel)
        {
            var userModel = new UserModel
            {
                FullName = registerModel.FullName,
                Email = registerModel.Email,
                UserName = registerModel.Email
            };

            return userModel;
        }

        public static UserViewModel ToViewModel(this UserModel userModel)
        {
            var userViewModel = new UserViewModel()
            {
                FullName = userModel.FullName,
                Email = userModel.Email,
                PhoneNumber = userModel.PhoneNumber,
                BillingAddress = userModel.BillingAddress?.ToViewModel(),
                DeliveryAddress = userModel.DeliveryAddress?.ToViewModel()
            };

            return userViewModel;
        }

        public static AddressViewModel ToViewModel(this Address address)
        {
            var addressViewModel = new AddressViewModel()
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

            return addressViewModel;
        }

        public static Address ToDbAddress(this AddressViewModel addressViewModel)
        {
            var address = new Address()
            {
                AddressLine1 = addressViewModel.AddressLine1,
                AddressLine2 = addressViewModel.AddressLine2,
                City = addressViewModel.City,
                Country = addressViewModel.Country,
                County = addressViewModel.County,
                PostalCode = addressViewModel.PostalCode,
                PhoneNumber = addressViewModel.PhoneNumber,
                FullName = addressViewModel.FullName,
                AddressType = addressViewModel.AddressType == AddressTypeViewEnum.Delivery ? AddressType.Delivery : AddressType.Billing
            };

            return address;
        }
    }
}
