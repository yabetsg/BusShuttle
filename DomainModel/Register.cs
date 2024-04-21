namespace DomainModel
{
    public class Register
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }



        public Register(int id, string userName, string password,string confirmPassword)
        {
            Id = id;
            UserName = userName;
            Password = password;
            ConfirmPassword = confirmPassword;
        }

        public void Update(string userName, string password,string confirmPassword)
        {
            UserName = userName;
            Password = password;
            ConfirmPassword = confirmPassword;
        }
    }
}
