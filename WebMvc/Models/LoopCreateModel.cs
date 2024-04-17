using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class LoopCreateModel
    {

        [Display(Name = "Loop Name")]
        public string Name { get; set; }

        public static LoopCreateModel FromLoop(Loop loop)
        {
            return new LoopCreateModel
            {
                Name = loop.Name,
            };
        }
    }
}
