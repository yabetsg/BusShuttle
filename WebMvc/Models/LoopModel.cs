using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class LoopModel
    {
        [Key]
        public int Id { get; set;}
        public string Name { get; set;}

    }
}
