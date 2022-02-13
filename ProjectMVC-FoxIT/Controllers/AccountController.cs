using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        private IMapper _mapper;

        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, WorkOrdersContext context, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _roleManager = roleManager;
            _mapper = mapper;
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
                return View(model);

            }
            return RedirectToAction("Index", "WorkOrders");


        }


        [HttpGet]
        public IActionResult Register()
        {
            Register model = new Register();
            var userRolesToList = _roleManager.Roles.ToList();
            model.Roles = new SelectList(userRolesToList, "Name", "Name");

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Register(Register model)
        {
            try
            {
                var userRolesToList = _roleManager.Roles.ToList();
                model.Roles = new SelectList(userRolesToList, "Name", "Name");

                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var email = _context.AspNetUsers.FirstOrDefault(x => x.Email == model.Email);
                if (email != null)
                {
                    ModelState.AddModelError("Error", "Побараната Е-маил адреса е веќе искористена, ве молиме внесете друга Е-маил адреса!");
                    return View(model);
                }
                IdentityUser user = new IdentityUser()
                {
                    Email = model.Email,
                    UserName = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.TryAddModelError(error.Code, error.Description);
                    }
                    return View(model);
                }
                await _userManager.AddToRoleAsync(user, model.Role);

                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", ex.Message.ToString() + "\n ex:" + ex);
                return View(model);
            }

        }
    }
}
