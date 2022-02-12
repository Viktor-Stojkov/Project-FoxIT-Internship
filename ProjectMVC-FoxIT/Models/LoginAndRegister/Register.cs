using Microsoft.AspNetCore.Mvc.Rendering;

namespace ProjectMVC_FoxIT.Models.LoginAndRegister
{
    public class Register
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public SelectList Roles { get; set; }
    }
}
