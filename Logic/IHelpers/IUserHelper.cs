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
        string UpdatePaymentInfo(int orderId);
        string DeclinePaymentInfo(int orderId);
    }
}
