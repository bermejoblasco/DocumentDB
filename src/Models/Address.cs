using System.ComponentModel.DataAnnotations;

namespace todo.Models
{
    public class Address
    {
        [Display(Name = "State")]
        public string State { get; set; }
        [Display(Name = "Country")]
        public string County { get; set; }
        [Display(Name = "City")]
        public string City { get; set; }

    }
}