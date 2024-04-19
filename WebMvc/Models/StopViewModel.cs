using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class StopViewModel
    {
        public int Id { get; set;}
        [Display(Name = "Stop Name")]
        public string Name { get; set; }

        public static StopViewModel FromStop(Stop stop)
        {
            return new StopViewModel
            {
                Id = stop.Id,
                Name = stop.Name,
            };
        }
    }
}
