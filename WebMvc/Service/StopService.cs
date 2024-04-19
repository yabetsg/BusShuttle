using System.Collections.Generic;
using System.Linq;
using WebMvc.Models;
using DomainModel;


namespace WebMvc.Service
{
    public class StopService : IStopService
    {
        private BusContext busDb;

        public StopService(BusContext busDb)
        {
            this.busDb = busDb;
        }

        public List<Stop> GetAllStops()
        {
            var stops = busDb.Stops
                .Select(e => new Stop(e.Id, e.StopName))
                .ToList();
            return stops;
        }

        public void CreateStop(string stopName)
        {
            var stops = GetAllStops();
            busDb.Add(new StopModel { Id = stops.Count+1, StopName = stopName });
            busDb.SaveChanges();
        }

        public Stop? FindStopByID(int id)
        {
            var allStops = GetAllStops();
            return allStops.Find(loop => loop.Id == id);
        }

        public void UpdateStopByID(int id, string loopName)
        {
            var stops = GetAllStops();
            var stop = busDb.Stops.FirstOrDefault(e => e.Id == id);
            if (stop != null)
            {
                stop.Id = id;
                stop.StopName = loopName;

                busDb.SaveChanges();
            }
        }

    }
}
