using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class RegisterModel
    {
        [Key]
        public int Id { get; set;}
        public string UserName { get; set;}
        public string Password { get; set;}

    }
}
