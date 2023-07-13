using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using FoodBar.Models;
using Logic.Helpers;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography;

namespace FoodBar.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserHelper _userHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;


        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserHelper userHelper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _context = context;

        }

        [HttpPost]
        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Index()
        {
            return View();
        }

        //GET
        [HttpGet]
        public IActionResult Registration()
        {
            var model = new RegistrationViewModel();
            model.IsAdmin = false;
            return View(model);
        }
        
        [HttpGet]
        public IActionResult AdminRegistration()
        {
            var model = new RegistrationViewModel();
            model.IsAdmin = true;
            return View(model);
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Registration(RegistrationViewModel model)
        {
            if (model != null)
            {
                var userEmail = await _userHelper.FindByEmailAsync(model.Email).ConfigureAwait(false);
                if (userEmail != null)
                {
                    SetMessage("Email already exits", Message.Category.Error); 
                    return View(model);
                }
                if (model.password != model.Confirmpassword)
                {
                    SetMessage("Password does not match", Message.Category.Error);
                    return View(model);

                }
                var saveUser = await _userHelper.Registration(model).ConfigureAwait(false);
                if (saveUser != null)
                {
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(model);

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userHelper.FindByEmailAsync(model.Email).ConfigureAwait(false);
                    if (user != null)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);

                        if (result.Succeeded)
                        {
                            var admin = await _userManager.IsInRoleAsync(user, "Admin").ConfigureAwait(false);
                            if (admin)
                            {
                                return RedirectToAction("Index", "Admin");
                            }
                            else
                            {
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }

                return View(model);
            }
            catch (Exception x)
            {
                throw x;
            }

        }
      
    }
}