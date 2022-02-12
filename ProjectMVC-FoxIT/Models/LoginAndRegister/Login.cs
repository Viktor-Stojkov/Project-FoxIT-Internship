using System.ComponentModel.DataAnnotations;

namespace ProjectMVC_FoxIT.Models.LoginAndRegister
{
    public class Login
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
