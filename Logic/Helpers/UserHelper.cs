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
                        await _userManager.AddToRoleAsync(newAppUser, "User").ConfigureAwait(false);
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

       public bool UpdatePaymentInfo(string userId,int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var payment = _context.Payments.Where(x=> x.OrderId == orderId && x.UserId == userId && !x.Deleted).FirstOrDefault();
                if (payment != null)
                {
                    payment.status = PaymentStatus.Seen;

                    _context.Payments.Update(payment);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool UpdateOrderInfo(string userId, int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var order = _context.Orders.Where(o => o.UserId == userId && o.Id == o.Id).FirstOrDefault();
                if (order != null)
                {
                    order.status = PaymentStatus.Seen;
                    order.DateCreated = DateTime.Now;
                    order.Active = true;
                    order.Deleted = false;

                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeclinePaymentInfo(string userId, int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var payment = _context.Payments.Where(x => x.OrderId == orderId && x.UserId == userId && !x.Deleted).FirstOrDefault();
                if (payment != null)
                {
                    payment.status = PaymentStatus.Declined;

                    _context.Payments.Update(payment);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }

        public bool DeclineOrderInfo(string userId, int orderId)
        {
            if (userId != null && orderId > 0)
            {
                var order = _context.Orders.Where(o => o.UserId == userId && o.Id == o.Id).FirstOrDefault();
                if (order != null)
                {
                    order.status = PaymentStatus.Declined;
                    order.DateCreated = DateTime.Now;
                    order.Active = true;
                    order.Deleted = false;

                    _context.Orders.Update(order);
                    _context.SaveChanges();
                    return true;
                }
            }
            return false;
        }


    }
}




