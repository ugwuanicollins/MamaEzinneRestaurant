using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using FoodBar.Models;
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
        public IActionResult PaymentTable(int OrderId,/*string evidence,*/ string userName)
        {
            IEnumerable<Payment> payments = _context.Payments.Where(x => x.Id != 0 && !x.Deleted && x.Active).Include(x => x.User);
            return View(payments);
        }

        [HttpGet]
        public JsonResult GetPaymentById(int payId)
        {
            if (payId != 0)
            {
                var payment = _context.Payments.Where(x => x.Id == payId).Include(u => u.User).FirstOrDefault();
                if (payment != null)
                {
                    return Json(new { isError = false, data = payment });
                }
            }
            return Json(new { isError = true, msg = "Please add food " });
        }


        [HttpPost]
        public IActionResult Approve(string userId, int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var user = _userHelper.GetUserById(userId).Result;
                if (user != null)
                {
                    var paymentUpdate = _userHelper.UpdatePaymentInfo(userId, orderId);
                    var orderUpdate = _userHelper.UpdateOrderInfo(userId, orderId);
                    _emailHelper.SendApprovalEmailToCustomer(user);
                }
                return Json(new { isError = false, msg = "Payment is approve Successfully." });
            }
            return Json(new { isError = true, msg = "unable to approve payment." });
        }

        public IActionResult Decline(string userId, int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var user = _userHelper.GetUserById(userId).Result;
                if (user != null)
                {
                    var declinePayment = _userHelper.DeclinePaymentInfo(userId,orderId);
                    var declineOrder = _userHelper.DeclineOrderInfo(userId, orderId);
                    _emailHelper.SendDeclinationEmailToCustomer(user);
                }
                return Json(new { isError = false, msg = "Payment is Declined." });
            }
            return Json(new { isError = true, msg = "Error Occurred" });
        }

        [HttpGet]
        public IActionResult KeepRecord()
        {
            ViewBag.foods = _foodHelper.GetFoodDropdown();
            var salesRecordViewModel = new List<SalesRecordViewModel>();
            var records = _context.SalesRecords.Where(s => s.FoodId > 0 && s.UserId != null && !s.IsDeleted)
                .Include(s => s.User).Include(s => s.Foods).ToList();
            if (records.Any())
            {
                foreach (var record in records)
                {
                    var recordView = new SalesRecordViewModel() 
                    {
                     FoodId = record.FoodId,
                     FoodName = record.Foods?.Name,
                     UserId = record.UserId,
                     UserName = record.User?.Email,
                     RecordDate = record.RecordDate,
                     Price = record.Price,
                     Quantity = record.Quantity,
                     Total = record.Total,
                     GrandTotal = record.GrandTotal
                    };
                    salesRecordViewModel.Add(recordView);   
                }
                return View(salesRecordViewModel);
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

        public IActionResult GetOrdersByDataCreated(DateTime selectedDate)
        {
            try
            {
                if (selectedDate != DateTime.MinValue)
                {
                    var orders = _context.Orders.Where(x => x.Id > 0 && x.UserId != null && x.FoodId > 0 && x.DateCreated.Date == selectedDate.Date).Include(x => x.Foods).Include(x => x.User).ToList();
                    if (orders != null && orders.Any())
                    {
                        
                        var foods = orders.Select(x=> x.Foods).Distinct();
                        var users = orders.Select(x => x.User).Distinct();
                      var price =  foods.Where(c => c.Id > 0 && c.Price != null).FirstOrDefault().Price;

                        return Json(new { isError = false,data = orders,foods = foods,users = users,price = price});
                    }
                    return Json(new { isError = true, msg = "Input Required" });
                }
                return Json(new { isError = true, msg = "Input Required" });
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

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

        [HttpPost]
        public JsonResult SaveRecordTable(int foodId, double price, DateTime recordDate, string user, double total, int quantity)
        {
            try
            {
                if (foodId != 0)
                {
                    var salesRecord = _foodHelper.SaveSalesRecord(foodId,price,recordDate, user, total,quantity);
                    if (salesRecord != null)
                    {
                        return Json(new { isError = false, msg = "SalesRecord was added Successfully." });
                    }
                    return Json(new { isError = true, msg = "Something went wrong." });
                }
                return Json(new { isError = true, msg = "Error Occured." });
            }
            catch (Exception exp)
            {

                throw exp;
            }
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
                    SetMessage("Name is Required", Message.Category.Error);
                    return View(model);
                }
                if (foodViewModel.Price <= 0)
                {
                    SetMessage("Price is Required", Message.Category.Error);
                    return View(model);
                }
                if (foodViewModel.Description == null && string.IsNullOrEmpty(foodViewModel.Description))
                {
                    SetMessage("Description is Required", Message.Category.Error);
                    return View(model);
                }
                if (foodViewModel.ImageUrl == null)
                {
                    SetMessage("Image is Required", Message.Category.Error);
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
            return _context.Foods.Where(x => x.Id != 0 && !x.IsDeleted).OrderByDescending(X => X.DateCreated).ToList();
        }
    }

}

