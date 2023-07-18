using Logic.IHelpers;
using Core.DATABASE;
using Core.Models;
using System;
using Microsoft.AspNetCore;
using Core.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Core.Enums;
using Newtonsoft.Json;

namespace Logic.Helpers
{
    public class UserHelper : IUserHelper
    {
        private readonly AppDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        //private readonly IWebHostEnvironment _hostEnvironment;

        public UserHelper(AppDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            //_hostEnvironment = hostEnvironment;
        }

        public ApplicationUser FindByUserName(string username)
        {
            return _userManager.Users.Where(s => s.UserName == username)?.FirstOrDefault();
        }
        public async Task<ApplicationUser> FindByUserNameAsync(string username)
        {
            return await _userManager.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            return await _userManager.Users.Where(s => s.Email == email)?.FirstOrDefaultAsync();
        }
        public async Task<ApplicationUser> GetUserById(string userId)
        {
            return await _userManager.Users.Where(s => s.Id == userId)?.FirstOrDefaultAsync();
        }

        public async Task<ApplicationUser?> Registration(RegistrationViewModel applicationUserViewModel)
        {
            try
            {
                if (applicationUserViewModel != null)
                {
                    var newAppUser = new ApplicationUser();

                    newAppUser.UserName = applicationUserViewModel.Email;
                    newAppUser.Email = applicationUserViewModel.Email;
                    newAppUser.DateCreated = DateTime.Now;
                    newAppUser.IsDeleted = false;
                    newAppUser.IsActive = true;

                    var newResult = await _userManager.CreateAsync(newAppUser, applicationUserViewModel.password);
                    if (newResult.Succeeded)
                    {
                        if (applicationUserViewModel.IsAdmin)
                        {
                            await _userManager.AddToRoleAsync(newAppUser, "Admin");
                        }
                        else
                        {
                            await _userManager.AddToRoleAsync(newAppUser, "User");
                        }
                        return newAppUser;
                    }
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }




        }

       public string UpdatePaymentInfo(int orderId)
        {
            if (orderId > 0)
            {
                var payment = _context.Payments.Where(x=> x.OrderId == orderId && !x.Deleted).Include(x => x.Orders).Include(x=> x.Orders.User).FirstOrDefault();
                if (payment != null)
                {
                    payment.status = PaymentStatus.Seen;

                    payment.Orders.OrderObject = JsonConvert.DeserializeObject<OrderItems[]>(payment.Orders.OrderDetails);
                    SaveSalesRecord(payment.Orders);

                    _context.Payments.Update(payment);
                    _context.SaveChanges();
                    return payment.Orders.User.Email;
                }
            }
            return null;
        }

        public string DeclinePaymentInfo(int orderId)
        {
            if (orderId > 0)
            {
                var payment = _context.Payments.Where(x => x.OrderId == orderId && !x.Deleted).Include(x=> x.Orders.User).FirstOrDefault();
                if (payment != null)
                {
                    payment.status = PaymentStatus.Declined;

                    payment.Orders.OrderObject = JsonConvert.DeserializeObject<OrderItems[]>(payment.Orders.OrderDetails);
                    SaveSalesRecord(payment.Orders);

                    _context.Payments.Update(payment);
                    _context.SaveChanges();
                    return payment.Orders.User.Email;
                }
            }
            return null;
        }

        public async Task<bool> SaveSalesRecord(Order data)
        {
            if (data != null)
            {
                var sales = new List<SalesRecord>();
                foreach (var order in data.OrderObject)
                {
                    var salesRecord = new SalesRecord()
                    {
                        FoodId = order.FoodId,
                        OrderId = data.Id,
                        Price = order.UnitPrice,
                        RecordDate = data.DateCreated,
                        Total = order.total,
                        Quantity = order.Quantity,
                        IsActive = true,
                        IsDeleted = false,
                    };
                    sales.Add(salesRecord);
                }
                _context.SalesRecords.AddRange(sales);
                return true;
            }
            return false;
        }
    }
}




