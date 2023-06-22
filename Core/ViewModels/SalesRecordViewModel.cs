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
        public string? UserId { get; set; }
        public int? FoodId { get; set; }
        public string? FoodName { get; set; }
        public string? UserName { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public double GrandTotal { get; set; }
        public DateTime RecordDate { get; set; }
    }
}
