namespace DomainModel
{
    public class Entry
    {
        public int Id { get; set; }
        public int BusNumber { get; set; }
        public string DriverName { get; set; }
        public string LoopName { get; set; }
        public string StopName { get; set; }
        public int Boarded { get; set; }
        public int LeftBehind{get; set; }

        public DateTime TimeStamp{get; set; }
        public Entry(int id, int busNumber, string driverName, string loopName, string stopName, int boarded,int leftBehind,DateTime timeStamp)
        {
            Id = id;
            BusNumber = busNumber;
            DriverName = driverName;
            LoopName = loopName;
            StopName = stopName;
            Boarded = boarded;
            LeftBehind = leftBehind;
            TimeStamp = timeStamp;
        }

        public void Update(int id, int boarded,int leftBehind,DateTime timeStamp)
        {
            Id = id;
            Boarded = boarded;
            LeftBehind = leftBehind;
            TimeStamp = timeStamp;
        }
    }
}
