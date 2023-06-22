using Core.DATABASE;
using Core.Migrations;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using MailKit.Search;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace FoodBar.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUserHelper _userHelper;
        private readonly IFoodHelper _foodHelper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IOrderHelper _orderHelper;


        public OrderController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IOrderHelper orderHelper, IUserHelper userHelper, IFoodHelper foodHelper, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;
            _foodHelper = foodHelper;
            _context = context;
            _orderHelper = orderHelper;

        }

        [HttpGet]
        public IActionResult Index(int foodId)
        {
            var username = User.Identity.Name;
            var user = _userHelper.FindByUserName(username);
            var order = _context.Orders.Where(x => x.Id != 0 && !x.Deleted && x.UserId == user.Id).FirstOrDefault();
            if (order != null)
            {
                return View(order);
            }
            return View();
        }

        [HttpGet]
        public IActionResult IndexList(int foodId)
        {
            var orders = new List<Order>();
            var username = User.Identity.Name;
            var user = _userHelper.FindByUserName(username);
            if(user != null)
            {
                IEnumerable<Order> order = _context.Orders.Where(x => x.Id != 0 && !x.Deleted && x.UserId == user.Id).ToList();
                return View(order);
            }

            return View(orders);
        }


        [HttpPost]
        public JsonResult OrderPayment(int foodId, string accountName, string bankName, string accountNumber,int quantity,double amount)
        {
            try
            {
                if (foodId != 0)
                {
                    var userName = (User.Identity.Name);
                    var referenceNumber = _orderHelper.GenerateOrderNumber();
                    if (referenceNumber != null && userName != null)
                    {
                        var order = _orderHelper.Order(foodId, referenceNumber, accountName, bankName, accountNumber, userName,quantity,amount);
                        if (order != null)
                        {
                            var payment = _orderHelper.UpdatePaymentTable(order.Id, /*evidence*/ userName);
                            return Json(new { isError = false, data = payment, msg = "Payment was added Successfully." });
                        }
                        return Json(new { isError = false, msg = "order payment was Successfully added." });
                    }
                    return Json(new { isError = true, msg = "Something went wrong." });
                }
                return Json(new { isError = true, msg = "error occured!" });
            }
            catch (Exception exp)
            {
                throw exp;
            }
        }

        [HttpPost]
        public IActionResult UploadEvidence(int orderId, int paymentId, string realfilepath)
        {
            var image = "/pic/" + realfilepath;
            var payments = _context.Payments.Where(x => x.OrderId == orderId && x.Id == paymentId && !x.Deleted).FirstOrDefault();
            if (payments != null)
            {
                payments.Evidences = image;
                _context.Update(payments);
                _context.SaveChanges();
                return Json(new { isError = false, msg = "Payment was added Successfully." });
            }
            return Json(new { isError = true, msg = "Something went wrong." });
        }

        [HttpGet]
        public IActionResult GetPriceByQuantity(int foodId,int quantity)
        {
            try
            {
                if (foodId > 0)
                {
                    var food = _context.Foods.Where(x => x.Id == foodId && !x.IsDeleted).FirstOrDefault();
                    if (food != null)
                    {
                        var quantityAmount = food.Price * quantity;
                        return Json(new { isError = false,data = quantityAmount});
                    }
                    return Json(new { isError = true, msg = "Unable to get the price." });
                }
                return Json(new { isError = true, msg = "Something went wrong." });
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }
    }
}

