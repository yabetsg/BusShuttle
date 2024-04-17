using DomainModel;
using System.ComponentModel.DataAnnotations;

namespace WebMvc.Models
{
    public class DriverCreateModel
    {

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public static DriverCreateModel FromDriver(Driver driver)
        {
            return new DriverCreateModel
            {
                FirstName = driver.FirstName,
                LastName = driver.LastName,
            };
        }
    }
}
