using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "FullName")]
        public string FullName { get; set; }
    }
}
