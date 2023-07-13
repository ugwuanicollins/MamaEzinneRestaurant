using Core.Enums;
using Core.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModels
{
    public class OrderViewModel
    {

        public int Id { get; set; }
        public double Amount { get; set; }
        public string? AccountName { get; set; }
        public string? BankName { get; set; }
        public string? ReferenceNumber { get; set; }
        public double AccountNumber { get; set; }
        public string? OrderDetails { get; set; }
        public DateTime DateCreated { get; set; }
        public string? FoodName { get; set; }
        public IFormFile? EvidenceUrl { get; set; }
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }
        public string? Description { get; set; }

    }
}
