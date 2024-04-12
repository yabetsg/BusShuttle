using System.Collections.Generic;
using System.Linq;
using WebMvc.Models;
using DomainModel;

namespace WebMvc.Service
{
    public class EntryService : IEntryService
    {
        private BusContext busDb;

        public EntryService(BusContext busDb)
        {
            this.busDb = busDb;
        }

        public List<Entry> GetAllEntries()
        {
            var entries = busDb.Entries
                .Select(e => new Entry(e.Id, e.BusNumber, e.DriverName, e.LoopName, e.StopName, e.Boarded,e.LeftBehind))
                .ToList();
            return entries;
        }

        public void CreateEntry(int busNumber, string driverName, string loopName, string stopName, int boarded,int leftBehind)
        {
            var entries = GetAllEntries();
            busDb.Add(new EntryModel { Id = entries.Count + 1, BusNumber = busNumber, DriverName = driverName, LoopName = loopName, StopName = stopName, Boarded = boarded , LeftBehind = leftBehind });
            busDb.SaveChanges();
        }

        public Entry? FindEntryByID(int id)
        {
            var allEntries = GetAllEntries();
            return allEntries.Find(entry => entry.Id == id);
        }

        public void UpdateEntryByID(int id, int busNumber,string driverName,string loopName, string stopName,int boarded,int leftBehind)
        {
            var entries = GetAllEntries();
            var entry = busDb.Entries.FirstOrDefault(e => e.Id == id);
            if (entry != null)
            {
                entry.Id = id;
                entry.BusNumber = busNumber;
                entry.DriverName = driverName;
                entry.LoopName = loopName;
                entry.StopName = stopName;
                entry.Boarded = boarded;
                entry.LeftBehind = leftBehind;
                busDb.SaveChanges();
            }
        }
       
    }
}
