
using DomainModel;

public interface IDriverService
{
    List<Driver> GetAllDrivers();
    void CreateDriver(string FirstName, string Lastname);
    Driver? FindDriverByID(int id);
    void UpdateDriverByID(int id,string FirstName,string LastName);
}

