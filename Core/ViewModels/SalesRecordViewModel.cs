using Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class SalesRecordViewModel
    {
        public string? FoodName { get; set; }
        public string? Email { get; set; }
         public string? OrderNumber { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public double Total { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
