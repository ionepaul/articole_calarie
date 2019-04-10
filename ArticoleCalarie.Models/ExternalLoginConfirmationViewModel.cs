using System.ComponentModel.DataAnnotations;
using ArticoleCalarie.Models.Utils;

namespace ArticoleCalarie.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nume complet")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Trebuie sa citesti si sa accepti politica de confidentialitate.")]
        [MustBeTrue(ErrorMessage = "Trebuie sa citesti si sa accepti politica de confidentialitate.")]
        public bool IsTermsAccepted { get; set; }

        [Required(ErrorMessage = "Trebuie sa citesti si sa accepti politica de confidentialitate.")]
        [MustBeTrue(ErrorMessage = "Trebuie sa citesti si sa accepti politica de confidentialitate.")]
        public bool IsPrivacyPolicyAccepted { get; set; }

        public bool IsNewsletterSubscription { get; set; }

        public string ReturnUrl { get; set; }
    }
}
