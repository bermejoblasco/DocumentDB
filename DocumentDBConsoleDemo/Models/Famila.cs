using System.Collections.Generic;

namespace DocumentDBConsoleDemo.Models
{
    public class Famila
    {
        //public string Id { get; set; }
        public string Apellido { get; set; }
        public List<Padre> Padres { get; set; }
        public List<Hijo> Hijos { get; set; }
        public Address Direccion { get; set; }
    }
}