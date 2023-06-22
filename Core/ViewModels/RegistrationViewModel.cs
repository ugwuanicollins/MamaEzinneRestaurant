using System.ComponentModel.DataAnnotations;

namespace Core.ViewModels
{
    public class RegistrationViewModel
    {
        [Key]
        [Required(ErrorMessage = "Please Enter Email...")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Please Enter Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? password { get; set; }

        [Required(ErrorMessage = "Please Enter the Confirm Password...")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string? Confirmpassword { get; set; }

    }
}
