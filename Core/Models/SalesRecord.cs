using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SalesRecord:BaseModel
    {
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public int? FoodId { get; set; }
        [ForeignKey("FoodId")]
        public virtual Food? Foods { get; set; }
        public double Price { get; set; }
        public double Total { get; set; }
        public int Quantity { get; set; }
        public double GrandTotal { get; set; }
        public DateTime RecordDate { get; set; }
    }
}