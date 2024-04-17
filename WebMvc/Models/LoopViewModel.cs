using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class LoopViewModel
    {
        public int Id { get; set;}
        [Display(Name = "Loop Name")]
        public string Name { get; set; }

        public static LoopViewModel FromLoop(Loop loop)
        {
            return new LoopViewModel
            {
                Id = loop.Id,
                Name = loop.Name,
            };
        }
    }
}
