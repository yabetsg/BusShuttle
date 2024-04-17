using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class DriverModel
    {
        [Key]
        public int Id { get; set;}
        public string FirstName { get; set;}
        public string LastName { get; set;}

    }
}
