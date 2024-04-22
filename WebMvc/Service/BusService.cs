using System.Collections.Generic;
using System.Linq;
using WebMvc.Models;
using DomainModel;


namespace WebMvc.Service
{
    public class BusService : IBusService
    {
        private BusContext busDb;

        public BusService(BusContext busDb)
        {
            this.busDb = busDb;
        }

        public List<Bus> GetAllBuses()
        {
            var buses = busDb.Buses
                .Select(e => new Bus(e.Id, e.BusNumber))
                .ToList();
            return buses;
        }

        public void CreateBus(int busNumber)
        {
            var buses = GetAllBuses();
            busDb.Add(new BusModel { Id = buses.Count + 1, BusNumber = busNumber });
            busDb.SaveChanges();
        }

        public Bus? FindBusByID(int id)
        {
            var allBuses = GetAllBuses();
            return allBuses.Find(bus => bus.Id == id);
        }

        public void UpdateBusByID(int id, int busNumber)
        {
            var buses = GetAllBuses();
            var bus = busDb.Buses.FirstOrDefault(e => e.Id == id);
            if (bus != null)
            {
                bus.Id = id;
                bus.BusNumber = busNumber;
                busDb.SaveChanges();
            }
        }
        public void DeleteBus(int id)
        {
            var bus = busDb.Buses.FirstOrDefault(e => e.Id == id);
            if (bus != null)
            {
                busDb.Buses.Remove(bus);
                busDb.SaveChangesAsync();
            }
        }

    }
}
