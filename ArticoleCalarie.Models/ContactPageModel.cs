using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ContactPageModel
    {
        [Required(ErrorMessage = "Numele este obligatoriu.")]
        [Display(Name = "Nume")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [EmailAddress(ErrorMessage = "Email-ul introdus este invalid.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Mesajul este obligatoriu.")]
        [Display(Name = "Mesaj")]
        public string Message { get; set; }
    }
}
