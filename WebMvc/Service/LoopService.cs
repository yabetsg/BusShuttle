using System.Collections.Generic;
using System.Linq;
using WebMvc.Models;
using DomainModel;


namespace WebMvc.Service
{
    public class LoopService : ILoopService
    {
        private BusContext busDb;

        public LoopService(BusContext busDb)
        {
            this.busDb = busDb;
        }

        public List<Loop> GetAllLoops()
        {
            var loops = busDb.Loops
                .Select(e => new Loop(e.Id, e.Name))
                .ToList();
            return loops;
        }

        public void CreateLoop(string loopName)
        {
            var loops = GetAllLoops();
            busDb.Add(new LoopModel { Id = loops.Count+1, Name = loopName });
            busDb.SaveChanges();
        }

        public Loop? FindLoopByID(int id)
        {
            var allLoops = GetAllLoops();
            return allLoops.Find(loop => loop.Id == id);
        }

        public void UpdateLoopByID(int id, string loopName)
        {
            var loops = GetAllLoops();
            var loop = busDb.Loops.FirstOrDefault(e => e.Id == id);
            if (loop != null)
            {
                loop.Id = id;
                loop.Name = loopName;

                busDb.SaveChanges();
            }
        }

        public void DeleteLoop(int id)
        {
            var loop = busDb.Loops.FirstOrDefault(e => e.Id == id);
            if (loop != null)
            {
                busDb.Loops.Remove(loop);
                busDb.SaveChangesAsync();
            }
        }

    }
}
