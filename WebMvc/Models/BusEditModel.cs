using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class BusEditModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Bus Number")]
        public int BusNumber { get; set; }

        public static BusEditModel FromBus(Bus bus)
        {
            return new BusEditModel
            {
                Id = bus.Id,
                BusNumber = bus.BusNumber,
            };
        }
    }
}
