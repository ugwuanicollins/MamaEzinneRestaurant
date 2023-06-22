using Core.Enums;
using Core.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.Amqp.Framing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Order
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public DateTime DateCreated { get; set; }
        public  PaymentStatus status { get; set; }     
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? ReferenceNumber { get; set; }
        public string? AccountNumber { get; set; }
        public string? FoodName { get; set; }
        public int Quantity { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public int? FoodId { get; set; }
        [ForeignKey("FoodId")]
        public virtual Food? Foods { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
    }
 
}
