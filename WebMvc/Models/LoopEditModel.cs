using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class LoopEditModel
    {
        public int Id { get; set;}
        [Display(Name = "Loop Name")]
        public string Name { get; set; }

        public static LoopEditModel FromLoop(Loop loop)
        {
            return new LoopEditModel
            {
                Name = loop.Name,
                Id = loop.Id,
            };
        }
    }
}
