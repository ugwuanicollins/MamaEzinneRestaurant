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
        Order Order(OrderViewModel orderData);
        Payment UpdatePaymentTable(Order order);
        string GenerateOrderNumber();
        Payment PreviewOrder(int payId);


    }
}
