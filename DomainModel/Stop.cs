namespace DomainModel
{
    public class Stop
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public Stop(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void Update(string name)
        {
            Name = name;
        }
    }
}
