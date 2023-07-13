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
        Order Order(OrderViewModel orderData);
        Payment UpdatePaymentTable(Order order);

        string GenerateOrderNumber();
        //bool OrderDetails(OrderViewModel orderDetails);


    }
}
