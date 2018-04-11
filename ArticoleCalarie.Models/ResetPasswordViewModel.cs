using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email-ul introdus este invalid.")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "{0} trebuie sa aiba cel putin {2} caractere.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirma parola")]
        [Compare("Password", ErrorMessage = "Parola si confirmarea parolei nu se potrivesc.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
