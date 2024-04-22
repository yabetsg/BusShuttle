
using DomainModel;

public interface ILoopService
{
    List<Loop> GetAllLoops();
    void CreateLoop(string Name);
    Loop? FindLoopByID(int id);
    void UpdateLoopByID(int id,string Name);
    void DeleteLoop(int id);
}

