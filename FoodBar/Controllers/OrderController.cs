using Core.DATABASE;
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
        public IActionResult Index(int payId)
        {
            if (payId > 0)
            {
                var order = _orderHelper.PreviewOrder(payId);
                return View(order);
            }
            return RedirectToAction("Index", "Food");
        }

        [HttpGet]
        public IActionResult IndexList()
        {
            var orders = new List<Payment>();
            var username = User.Identity.Name;
            var user = _userHelper.FindByUserName(username);
            if (user != null)
            {
                var order = _context.Payments.Where(x => x.Id != 0).Include(o => o.Orders).Where(x => x.Orders.UserId == user.Id).ToList();
                if (order != null)
                {
                    return View(order);
                }
            }
            return View(orders);
        }

        [HttpGet]
        public async Task<IActionResult> PreviewOrder(int id)
        {
            var order = _orderHelper.PreviewOrder(id);
            return PartialView(order);
        }

        [HttpPost]
        public JsonResult OrderPayment(OrderViewModel orderData)
        {
            try
            {
                if (orderData != null)
                {
                    var userName = (User.Identity.Name);
                    if (userName != null)
                    {
                        orderData.UserName = userName;
                        var order = _orderHelper.Order(orderData);
                        if (order != null)
                        {
                            var payment = _orderHelper.UpdatePaymentTable(order);
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
        public IActionResult UploadEvidence(int orderId, int paymentId, IFormFile picture)
        {
            var image = _foodHelper.ProcessFoodImage(picture);
            var payments = _context.Payments.Where(x => x.OrderId == orderId && x.Id == paymentId && !x.Deleted).FirstOrDefault();
            if (payments != null)
            {
                payments.Evidences = image;
                _context.Update(payments);
                _context.SaveChanges();
                return Json(new { isError = false, msg = "Payment was added Successfully.", payId = payments.Id });
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

