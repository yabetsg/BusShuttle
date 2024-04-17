using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class DriverEditModel
    {
        public int Id { get; set; }
        
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public static DriverEditModel FromDriver(Driver driver)
        {
            return new DriverEditModel
            {
                Id = driver.Id,
                FirstName = driver.FirstName,
                LastName = driver.LastName,
            };
        }
    }
}
