using Core.DATABASE;
using Core.Models;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodBar.Controllers
{
    public class CartController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IFoodHelper _foodHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public CartController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserHelper userHelper, IFoodHelper foodHelper, AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _foodHelper = foodHelper;
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }


        [HttpGet]
        public IActionResult AddToCart(string foodIds)
        {
            try
            {
                var ids = foodIds.Split(",");
                List<int> intList = ids.Select(s =>
                {
                    int result;
                    return int.TryParse(s, out result) ? result : 0;
                }).ToList();

                if (ids.Any())
                {
                    var model = _context.Foods.Where(x => intList.Contains(x.Id)).ToList();
                    return View(model);
                }
                return View();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

    }
}
