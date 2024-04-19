using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class BusModel
    {
        [Key]
        public int Id { get; set;}
        public int BusNumber { get; set;}
    }
}
