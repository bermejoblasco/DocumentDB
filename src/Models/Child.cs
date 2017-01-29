using System.ComponentModel.DataAnnotations;

namespace todo.Models
{
    public class Child
    {
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }
        [Display(Name = "Gender")]
        public string Gender { get; set; }
        [Display(Name = "Grade")]
        public int Grade { get; set; }
        public Pet Pets { get; set; }

    }
}