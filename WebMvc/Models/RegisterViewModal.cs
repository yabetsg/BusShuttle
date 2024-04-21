using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class RegisterViewModel
    {
        public int Id { get; set;}

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "Password")]
        public string Password { get; set; }

        public static RegisterViewModel FromRegister(Register register)
        {
            return new RegisterViewModel
            {
                Id = register.Id,
                UserName = register.UserName,
                Password = register.Password,
            };
        }
    }
}
