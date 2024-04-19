using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class StopEditModel
    {
        public int Id { get; set;}
        [Display(Name = "Stop Name")]
        public string Name { get; set; }

        public static StopEditModel FromStop(Stop stop)
        {
            return new StopEditModel
            {
                Name = stop.Name,
                Id = stop.Id,
            };
        }
    }
}
