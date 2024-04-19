using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class BusCreateModel
    {

        [Display(Name = "Bus Number")]
        public int BusNumber { get; set; }

        public static BusCreateModel FromBus(Bus bus)
        {
            return new BusCreateModel
            {

                BusNumber = bus.BusNumber,
            };
        }
    }
}
