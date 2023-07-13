using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class CartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public List<Cart> Carts { get; set; }
    }
}
