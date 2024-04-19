
using DomainModel;

public interface IStopService
{
    List<Stop> GetAllStops();
    void CreateStop(string stopName);
    Stop? FindStopByID(int id);
    void UpdateStopByID(int id,string stopName);
}

