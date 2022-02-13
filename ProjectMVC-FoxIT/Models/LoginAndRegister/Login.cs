using System.ComponentModel.DataAnnotations;

namespace ProjectMVC_FoxIT.Models.LoginAndRegister
{
    public class Login
    {
        [Required(ErrorMessage = "Корисничко име е задолжително поле")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Лозинката е задолжително поле")]
        public string Password { get; set; }
    }
}
