using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models;
using Core.ViewModels;

namespace Logic.IHelpers
{
    public interface IOrderHelper
    {
        //Task<Order> CreatePaystackPayment(string UserId, double amount, ApplicationUser user, int orderId);
        Order Order(int foodId, string referenceNumber, string accountName, string bankName, string accountNumber,string userName, int quantity,double amount);
        Payment UpdatePaymentTable(int orderId, /*string evidence,*/ string userName);

        string GenerateOrderNumber();
        //bool OrderDetails(OrderViewModel orderDetails);


    }
}
