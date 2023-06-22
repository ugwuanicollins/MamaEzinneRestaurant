using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class JobApplication
    {
        [Key]
       
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }

        public int? WorkDetailsId { get; set; }
        [ForeignKey("WorkDetailsId")]
        public virtual WorkDetails? WorkDetails { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
