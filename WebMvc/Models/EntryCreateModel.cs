using DomainModel;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class EntryCreateModel
    {

        public int Id { get; set; }
        
        [Display(Name = "Bus Number")]
        public int BusNumber { get; set; }
        [Display(Name = "Driver Name")]
        public string DriverName { get; set; }

        [Display(Name = "Loop Name")]
        public string LoopName { get; set; }



        [Display(Name = "Stop Name")]
        public string StopName { get; set; }


        [Display(Name = "Boarded")]
        public int Boarded { get; set; }


        [Display(Name = "Left Behind")]

        public int LeftBehind { get; set; }

        [Display(Name = "Time")]
        public DateTime TimeStamp { get; set; }
        public static EntryCreateModel FromEntry(Entry entry)
        {
            return new EntryCreateModel
            {
                Id = entry.Id,
                BusNumber = entry.BusNumber,
                DriverName = entry.DriverName,
                LoopName = entry.LoopName,
                Boarded = entry.Boarded,
                StopName = entry.StopName,
                LeftBehind = entry.LeftBehind,
                TimeStamp = entry.TimeStamp,
            };
        }
    }
}
