using System.ComponentModel.DataAnnotations;

namespace todo.Models
{
    public class Parent
    {
        [Display(Name = "Family name")]
        public string FamilyName { get; set; }
        [Display(Name = "First name")]
        public string FirstName { get; set; }

    }
}