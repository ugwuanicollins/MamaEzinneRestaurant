using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using FoodBar.Models;
using Logic.Helpers;
using Logic.IHelpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodBar.Controllers
{
    public class FoodController : BaseController
    {
        private readonly IUserHelper _userHelper;
        private readonly IFoodHelper _foodHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public FoodController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IUserHelper userHelper, IFoodHelper foodHelper, AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _foodHelper = foodHelper;
            _context = context;
            _hostingEnvironment = hostEnvironment;
        }


        [HttpGet]
        public IActionResult Index()
        {
            IEnumerable<Food> food = _context.Foods.Where(x => x.Id != 0 && !x.IsDeleted);
            return View(food);
        }

        // GET - ADDFOOD
        [HttpGet]
        public IActionResult AddFood()
        {

            return View();
        }


        [HttpPost]
        public async Task<ActionResult> AddFood(FoodViewModel foodViewModel)
        {
            var user = (User.Identity.Name);
            if (user != null && foodViewModel != null && foodViewModel.Price > 0)
            {
                if (foodViewModel.Name == null && string.IsNullOrEmpty(foodViewModel.Name))
                {
                    SetMessage("Name is Required", Message.Category.Error);
                    return View(foodViewModel);
                }
                if (foodViewModel.Price <= 0)
                {
                    SetMessage("Price is Required", Message.Category.Error);
                    return View(foodViewModel);
                }
                if (foodViewModel.Description == null && string.IsNullOrEmpty(foodViewModel.Description))
                {
                    SetMessage("Description is Required", Message.Category.Error);
                    return View(foodViewModel);
                }
                if (foodViewModel.ImageUrl == null)
                {
                    SetMessage("Image is Required", Message.Category.Error);
                    return View(foodViewModel);
                }
                var foods = _foodHelper.AdminAddFood(foodViewModel, user);
                if (foods != null)
                {
                  return RedirectToAction("ManageFood", "Admin");
                }
            }
            return View(foodViewModel);
        }

        [HttpGet]
        public async Task<JsonResult> GetFood(int foodId)
        {
            if (foodId > 0)
            {               
                var foods = await _foodHelper.GetFoodById(foodId).ConfigureAwait(false);
                if (foods != null)
                {
                    return Json(new { isError = false, data = foods, msg = "success" });
                }

                return Json(new { isError = true, msg = "error Occured" });
            }
            return Json(new { isError = true, msg = "error no id found" });
        }





    }
}








