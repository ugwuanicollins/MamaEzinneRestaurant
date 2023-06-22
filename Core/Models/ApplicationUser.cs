using Microsoft.AspNetCore.Identity;

namespace Core.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DateCreated { get; set; }
    }
}
