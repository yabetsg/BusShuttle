using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class RegisterCreateModel
    {

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]

        [Display(Name = "Confirm Password")]
        public string ConfirmPassword { get; set; }

        public static RegisterCreateModel FromRegister(Register register)
        {
            return new RegisterCreateModel
            {
                UserName = register.UserName,
                Password = register.Password,
                ConfirmPassword = register.ConfirmPassword,
            };
        }
    }
}
