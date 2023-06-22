using Microsoft.Build.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class WorkDetails:BaseModel
    {
        [Required]
        public double Salary { get; set; }
        public string? Email { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
