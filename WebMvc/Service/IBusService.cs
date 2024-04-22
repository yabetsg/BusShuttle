
using DomainModel;

public interface IBusService
{
    List<Bus> GetAllBuses();
    void CreateBus(int BusNumber);
    Bus? FindBusByID(int id);
    void UpdateBusByID(int id, int BusNumber);
    void DeleteBus(int id);
}

