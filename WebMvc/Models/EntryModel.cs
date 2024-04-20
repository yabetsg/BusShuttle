using DomainModel; 
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class EntryModel
    {
        [Key]
        public int Id { get; set;}
        public int BusNumber { get; set;}
        public string DriverName { get; set;}
        public string LoopName { get; set; }
        public string StopName { get; set; }
        public int Boarded { get; set;}
        public int LeftBehind{get; set; }
        public DateTime TimeStamp { get; set; }
    }
}
