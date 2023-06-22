using Core.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Payment
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string? ReferenceNumber { get; set; }
        public PaymentStatus status { get; set; }
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Orders { get; set; }
        public string? Evidences { get; set; }
        public DateTime DatePaid { get; set; }
        public bool Active { get; set; }
        public bool Deleted { get; set; }

    }
}
