using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class StopCreateModel
    {

        [Display(Name = "Stop Name")]
        public string Name { get; set; }

        public static StopCreateModel FromStop(Stop stop)
        {
            return new StopCreateModel
            {
                Name = stop.Name,
            };
        }
    }
}
