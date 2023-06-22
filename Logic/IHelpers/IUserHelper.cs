using Core.Models;
using Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.IHelpers
{
    public interface IUserHelper
    {
        Task<ApplicationUser> Registration(RegistrationViewModel applicationUserViewModel);
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task<ApplicationUser> FindByUserNameAsync(string username);
        ApplicationUser FindByUserName(string username);
        Task<ApplicationUser> GetUserById(string userId);
        bool UpdatePaymentInfo(string userId, int orderId);
        bool UpdateOrderInfo(string userId, int orderId);
        bool DeclinePaymentInfo(string userId, int orderId);
        bool DeclineOrderInfo(string userId, int orderId);


    }
}
