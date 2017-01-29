using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace todo.Models
{
    public class Family
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        [Display(Name = "Last name")]
        public string LastName { get; set; }
        [Display(Name = "Parents")]
        public Parent Parents { get; set; }
        [Display(Name = "Clildren")]
        public Child Children { get; set; }
        [Display(Name = "Address")]
        public Address Address { get; set; }
        [Display(Name = "Is registered")]
        public bool IsRegistered { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }

    }
}