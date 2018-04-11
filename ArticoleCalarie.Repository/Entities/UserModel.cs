using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ArticoleCalarie.Repository.Entities
{
    public class UserModel : IdentityUser 
    {
        public string FullName { get; set; }

        public int? BillingAddressId { get; set; }

        public int? DeliveryAddressId { get; set; }

        [ForeignKey("DeliveryAddressId")]
        public virtual Address DeliveryAddress { get; set; }

        [ForeignKey("BillingAddressId")]
        public virtual Address BillingAddress { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<UserModel> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            userIdentity.AddClaim(new Claim("FullName", this.FullName));

            // Add custom user claims here
            return userIdentity;
        }
    }
}
