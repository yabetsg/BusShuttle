namespace DomainModel
{
    public class Driver
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public Driver(int id, string firstName,string lastName)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
        }

        public void Update(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
    }
}
