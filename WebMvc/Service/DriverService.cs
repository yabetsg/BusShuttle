using System.Collections.Generic;
using System.Linq;
using WebMvc.Models;
using DomainModel;


namespace WebMvc.Service
{
    public class DriverService : IDriverService
    {
        private BusContext busDb;

        public DriverService(BusContext busDb)
        {
            this.busDb = busDb;
        }

        public List<Driver> GetAllDrivers()
        {
            var drivers = busDb.Drivers
                .Select(e => new Driver(e.Id, e.FirstName, e.LastName))
                .ToList();
            return drivers;
        }

        public void CreateDriver(string firstName, string lastName)
        {
            var drivers = GetAllDrivers();
            busDb.Add(new DriverModel { Id = drivers.Count + 1, FirstName = firstName, LastName = lastName });
            busDb.SaveChanges();
        }

        public Driver? FindDriverByID(int id)
        {
            var allDrivers = GetAllDrivers();
            return allDrivers.Find(driver => driver.Id == id);
        }

        public void UpdateDriverByID(int id, string firstName, string LastName)
        {
            var drivers = GetAllDrivers();
            var driver = busDb.Drivers.FirstOrDefault(e => e.Id == id);
            if (driver != null)
            {
                driver.Id = id;
                driver.FirstName = firstName;
                driver.LastName = LastName;
                busDb.SaveChanges();
            }
        }
          public void DeleteDriver(int id)
        {
            var driver = busDb.Drivers.FirstOrDefault(e => e.Id == id);
            if (driver != null)
            {
                busDb.Drivers.Remove(driver);
                busDb.SaveChangesAsync();
            }
        }

    }
}
