using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class EntryViewModel
    {
        public int Id { get; set;}
        
        [Display(Name = "Bus Number")]
        public int BusNumber { get; set;}
        [Display(Name = "Driver Name")]
        public string DriverName { get; set;}

        [Display(Name = "Loop Name")]
        public string LoopName { get; set; }

        [Display(Name = "Stop Name")]
        public string StopName { get; set; }

        [Display(Name = "Boarded")]
        public int Boarded { get; set;}
        public int LeftBehind{get; set; }


       public static EntryViewModel FromEntry(Entry entry)
        {
            return new EntryViewModel
            {
                Id = entry.Id,
                BusNumber = entry.BusNumber,
                DriverName = entry.DriverName,
                LoopName = entry.LoopName,
                Boarded = entry.Boarded,
                StopName = entry.StopName,
                LeftBehind = entry.LeftBehind,
            };
        }
    }
}