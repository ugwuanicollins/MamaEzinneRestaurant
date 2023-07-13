using Core.Enums;
using MessagePack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Cart
    {
        public List<CartItem> Items { get; set; }
        public Cart ()
        {
            Items = new List<CartItem>();
        }
    }
}
