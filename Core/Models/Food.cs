using Core.Enums;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Food:BaseModel
    {

        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser? User { get; set; }
        public DateTime DateCreated { get; set; }
        public string? Image { get; set; }
        public string? Description { get; set; }
        public double Price { get; set; }


    }
}
