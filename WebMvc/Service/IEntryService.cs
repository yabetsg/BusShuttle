
using DomainModel;

public interface IEntryService
{
    List<Entry> GetAllEntries();
    void CreateEntry(int busNumber, string driverName, string loopName, string stopName, int boarded,int leftBehind);
    Entry? FindEntryByID(int id);
    void UpdateEntryByID(int id, int busNumber,string driverName,string loopName, string stopName,int boarded,int leftBehind);
}

