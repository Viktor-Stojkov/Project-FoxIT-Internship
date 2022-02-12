using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProjectMVC_FoxIT.Data;
using ProjectMVC_FoxIT.Models.LoginAndRegister;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectMVC_FoxIT.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private WorkOrdersContext _context;
        private RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, WorkOrdersContext context, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(Login model)
        {
            try
            {
                if (!ModelState.IsValid || model == null)
                {

                    ModelState.AddModelError("Error", "Погрешен Е-маил или Лозинка. Ве молиме вметнете ги точните информации пред да се Логирате повторно!");
                    return View(model);
                }
                var user = _context.AspNetUsers.SingleOrDefault(x => x.UserName == model.Username);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: true);

                    if (!result.Succeeded || result.IsLockedOut || result.IsNotAllowed)
                    {
                        ModelState.AddModelError("Error", "Неуспешен обид за логирање. Ве молиме контактирајте ги нашите администратори!");
                        return View(model);
                    }
                }
                else
                {
                    ModelState.AddModelError("Error", "Корисникот не постои, Потребна е регистрација за да можете да се најавите!");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message.ToString() + "\n ex:" + ex);
            }
            return RedirectToAction("Index", "WorkOrders");
            
            
        }


        [HttpGet]
        public IActionResult Register()
        {
            Register register = new Register();

            var userRoles = _context.AspNetRoles.Select(x => x.Name).ToList();
            var userRolesToList = _roleManager.Roles.ToList();

            return View();
        }

        [HttpPost]
        public IActionResult Register(Register model)
        {
            return View(model);
        }
    }
}
