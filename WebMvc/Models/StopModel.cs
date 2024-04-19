using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class StopModel
    {
        [Key]
        public int Id { get; set;}
        public string StopName { get; set;}

    }
}
