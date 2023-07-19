using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using FoodBar.Models;
using Logic.Helpers;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace FoodBar.Controllers
{
    public class AdminController : BaseController
    {
        private readonly IUserHelper _userHelper;
        private readonly IFoodHelper _foodHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IOrderHelper _orderHelper;
        private readonly IEmailHelper _emailHelper;


        public AdminController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOrderHelper orderHelper, IUserHelper userHelper, IFoodHelper foodHelper, IEmailHelper emailHelper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _foodHelper = foodHelper;
            _context = context;
            _orderHelper = orderHelper;
            _emailHelper = emailHelper;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult PaymentTable(int payId)
        {
            IEnumerable<Payment> payments = _context.Payments.Where(x => x.Id != 0).Include(o => o.Orders);
            return View(payments);
        }

        [HttpGet]
        public JsonResult GetPaymentById(int payId)
        {
            if (payId != 0)
            {
                var payment = _context.Payments.Where(x => x.Id == payId)./*Include(u => u.User).*/FirstOrDefault();
                if (payment != null)
                {
                    return Json(new { isError = false, data = payment });
                }
            }
            return Json(new { isError = true, msg = "Please add food " });
        }


        [HttpPost]
        public IActionResult Approve(int orderId)
        {
            if ( orderId > 0)
            {
                var paymentEmail = _userHelper.UpdatePaymentInfo(orderId);
                if (paymentEmail != null)
                {
                    _emailHelper.SendApprovalEmailToCustomer(paymentEmail);
                    return Json(new { isError = false, msg = "Payment is approve Successfully." });
                }
            }
            return Json(new { isError = true, msg = "unable to approve payment." });
        }

        public IActionResult Decline(int orderId)
        {
            if (orderId > 0)
            {
                    var declinePayment = _userHelper.DeclinePaymentInfo(orderId);
                if (declinePayment != null)
                {
                    _emailHelper.SendDeclinationEmailToCustomer(declinePayment);
                    return Json(new { isError = false, msg = "Payment is Declined." });
                }
                 
            }
            return Json(new { isError = true, msg = "Error Occurred" });
        }

        [HttpGet]
        public IActionResult KeepRecord()
        {
            var salesRecordViewModel = new List<SalesRecordViewModel>();
            var records = _context.SalesRecords.Where(s => s.FoodId > 0 && !s.IsDeleted)
                .Include(s => s.Foods).Include(s => s.Order.User).ToList();
            if (records.Any())
            {
                foreach (var record in records)
                {
                    var recordView = new SalesRecordViewModel() 
                    {
                     FoodName = record.Foods?.Name,
                     Email = record.Order.User.Email,
                     OrderNumber = record.Order.ReferenceNumber,
                     Price = record.Price,
                     Quantity = record.Quantity,
                     Total = record.Total,
                     RecordDate = record.RecordDate,
                    };
                    salesRecordViewModel.Add(recordView);   
                }
            }
            return View(salesRecordViewModel);
        }

        [HttpGet] 
        public JsonResult GetFoodDetails(int foodId)
        {
            if (foodId != 0)
            {
                var foodDetails = _context.Foods.Where(x => x.Id == foodId && !x.IsDeleted).FirstOrDefault();
                if (foodDetails != null)
                {
                    return Json(new { isError = false,data = foodDetails});
                }
            }
            return Json(new { isError = true, msg = "Please add food " });
        }

        //public IActionResult GetOrdersByDataCreated(DateTime selectedDate)
        //{
        //    try
        //    {
        //        if (selectedDate != DateTime.MinValue)
        //        {
        //            var orders = _context.Orders.Where(x => x.Id > 0 && x.UserId != null && x.OrderDetails == null && x.DateCreated.Date == selectedDate.Date).Include(x => x.).Include(x => x.User).ToList();
        //            if (orders != null && orders.Any())
        //            {
                        
        //                var foods = orders.Select(x=> x.OrderDetails).Distinct();
        //                var users = orders.Select(x => x.User).Distinct();
        //              var price =  foods.Where(c => c.Id > 0 && c.Price != null).FirstOrDefault().Price;

        //                return Json(new { isError = false,data = orders,foods = foods,users = users,price = price});
        //            }
        //            return Json(new { isError = true, msg = "Input Required" });
        //        }
        //        return Json(new { isError = true, msg = "Input Required" });
        //    }
        //    catch (Exception exp)
        //    {

        //        throw exp;
        //    }
        //}

        [HttpGet]
        public JsonResult GetPriceDetails(int foodId)
        {
            if (foodId != 0)
            {
                var price = _context.Foods.Where(x => x.Id == foodId && x.Price != null && !x.IsDeleted).FirstOrDefault();
                if (price != null)
                {
                    return Json(new { isError = false, data = price});
                }
            }
            return Json(new { isError = true, msg = "Please add food " });
        }

        [HttpGet]
        public IActionResult ManageFood()
        {
            var model = new FoodViewModel();
            model.FoodList = GetAllFood();
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ManageFood(FoodViewModel foodViewModel)
        {
            var model = new FoodViewModel();
            model = foodViewModel;
            model.FoodList = GetAllFood();

            var user = (User.Identity.Name);
            if (user != null && foodViewModel != null && foodViewModel.Price > 0)
            {
                if (foodViewModel.Name == null && string.IsNullOrEmpty(foodViewModel.Name))
                {
                    return View(model);
                }
                if (foodViewModel.Price <= 0)
                {
                    return View(model);
                }
                if (foodViewModel.Description == null && string.IsNullOrEmpty(foodViewModel.Description))
                {
                    return View(model);
                }
                if (foodViewModel.ImageUrl == null)
                {
                    return View(model);
                }
                var foods = _foodHelper.AdminAddFood(foodViewModel, user);
                if (foods != null)
                {
                    return RedirectToAction("ManageFood", "Admin");
                }
            }

            return View(model);
        }

        public List<Food> GetAllFood()
        {
            return _context.Foods.Where(x => x.Id != 0 && !x.IsDeleted && x.IsActive ).OrderByDescending(X => X.DateCreated).ToList();
        }

        //Edit-GET
        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                return NotFound();
            }
            var food = _context.Foods.Find(id);
            if(food == null)
            {
                return NotFound();
            }
            return View(food);
        }

        //POST-EDIT
        [HttpPost]
        public JsonResult Edit(IFormFile picture, int id, string name, double price, string description)
        {
            var model = new FoodViewModel();

            model.Id = id;
            model.Name = name;
            model.ImageUrl = picture;
            model.Price = price;
            model.Description = description;
            var res = _foodHelper.EditFood(model); 
            if (res)
            {
                return Json(new { isError = false, msg = "Updated Successfully." });
            }
            return Json(new { isError = true, msg = "Something went wrong." });
        }

        [HttpPost]
        public JsonResult DeleteFood(int id)
        {
            if (id > 0)
            {
                var removeFood = _foodHelper.DeleteFood(id);
                if (removeFood)
                {
                    return Json(new { isError = false, msg = "Deleted Successfully." });
                }
                
            }
            return Json(new { isError = true, msg = "Something went wrong." });
        }

    
    }

}

 