namespace DomainModel
{
    public class Bus
    {
        public int Id { get; set; }
        public int BusNumber { get; set; }

        public Bus(int id, int busNumber)
        {
            Id = id;
            BusNumber = busNumber;
        }

        public void Update(int busNumber)
        {
            BusNumber = busNumber;
        }
    }
}
