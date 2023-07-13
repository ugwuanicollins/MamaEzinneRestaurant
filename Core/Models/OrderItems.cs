using Core.DATABASE;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class OrderItems
    {
        private readonly AppDbContext _context;
        public int Quantity { get; set; }
        public double total { get; set; }
        public double UnitPrice { get; set; }
        public int? FoodId { get; set; }
        public virtual Food? Foods { get; set; }
    }
}
