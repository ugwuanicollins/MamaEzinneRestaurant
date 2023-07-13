using Core.DATABASE;
using Core.Models;
using Core.ViewModels;
using Logic.IHelpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Collections.Specialized.BitVector32;
using Core.Enums;

namespace Logic.Helpers
{
    public class OrderHelper : IOrderHelper
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IUserHelper _userHelper;

        public OrderHelper(AppDbContext context, IUserHelper userHelper, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _userHelper = userHelper;

        }

        public string GenerateOrderNumber()
        {
            Random random = new Random();
            int randomNumber = random.Next(100000, 999999);
            return "Order" + randomNumber.ToString();
        }
        public Order Order(OrderViewModel orderData)
        {
            try
            {
                if (orderData != null)
                {
                    var user = _userHelper.FindByUserName(orderData.UserName);
                    var order = new Order()
                    {

                        UserId = user.Id,
                        Amount = orderData.Amount,
                        ReferenceNumber = GenerateOrderNumber(),
                        DateCreated = DateTime.Now,
                        AccountName = "Mama Ezinne Restaurant",
                        BankName = "Zenith Bank",
                        AccountNumber = "2553153651",
                        OrderDetails = orderData.OrderDetails,
                        Deleted = false,
                        Active = true
                    };

                    _context.Orders.Add(order);
                    _context.SaveChanges();
                    return order;

                }
                return null;
            }
            catch (Exception exp)
            {
                throw exp;
            }   
        }


        public Payment UpdatePaymentTable(Order order)
        {
            try
            {
                var payment = new Payment();
                if (order != null)
                {
                    payment.OrderId = order.Id;
                    payment.DatePaid = order.DateCreated;
                    payment.Active = true;
                    payment.Deleted = false;
                    payment.status = PaymentStatus.pending;


                    _context.Payments.Add(payment);
                    _context.SaveChanges();
                    return payment;
                }
                return null;
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }


       


    }
}


        //public async Task<Order> CreatePaystackPayment(string UserId, double amount, ApplicationUser user, int orderId)
        //{
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(UserId) && amount > 0)
        //        {
        //            var payments = _context.Payments.Where(x => x.Id == orderId && x.Active && !x.Deleted)
        //            .Include(x => x.User).Include(x => x.Orders).FirstOrDefault();
        //            if (payments != null)
        //            {
        //                var payment = new AdminPaymentRecord
        //                {

        //                    DatePaid = DateTime.Now,
        //                    UserId = UserId,
        //                    Name = payments.Name,
        //                    Orders = payments.Orders,
        //                    OrderId = orderId,
        //                    User = payments.User,
        //                    status = payments.status,
        //                    Amount = amount,
        //                    ReferenceNumber = GenerateOrderNumber(),
        //                    Active = true,
        //                    Deleted = false,

        //                };
        //                var newPayment = await _context.AddAsync(payment);
        //                await _context.SaveChangesAsync();
        //            }
        //        }
        //        return null;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //}



//        public bool OrderDetails(OrderViewModel orderDetails)
//        {
//            if (orderDetails != null)
//            {
//                var order = new Order();
//                var Details = _context.Foods.Where(x => x.Id == orderDetails.Id && x.IsActive && !x.IsDeleted).FirstOrDefault();
//                if (Details != null)
//                {
//                    Details.Name = orderDetails.FoodName;
//                    Details.Price = orderDetails.Amount;
//                    order.ReferenceNumber = GenerateOrderNumber();
//                    order.DateCreated = DateTime.Now;
//                    order.status = orderDetails.status;
//                    order.Deleted = false;
//                    order.Active = true;

//                    _context.Orders.Update(order);
//                    _context.SaveChanges();
//                    return true;
//                }
//            }
//            return false;
//        }



//    }
//}

