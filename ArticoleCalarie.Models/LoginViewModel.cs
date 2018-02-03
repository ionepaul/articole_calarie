﻿using System.ComponentModel.DataAnnotations;

namespace ArticoleCalarie.Models
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Email-ul este obligatoriu.")]
        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Parola este obligatorie.")]
        [DataType(DataType.Password)]
        [Display(Name = "Parola")]
        public string Password { get; set; }

        [Display(Name = "Ține-mă minte")]
        public bool RememberMe { get; set; }
    }
}
