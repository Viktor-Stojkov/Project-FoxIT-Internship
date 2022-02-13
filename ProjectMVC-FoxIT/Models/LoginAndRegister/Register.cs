using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ProjectMVC_FoxIT.Models.LoginAndRegister
{
    public class Register
    {
        [Required(ErrorMessage = "Емаил адресата е задолжителна")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Пасвордот е задолжителен")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Задолжително поле")]
        public string Role { get; set; }
        public SelectList Roles { get; set; }
    }
}
