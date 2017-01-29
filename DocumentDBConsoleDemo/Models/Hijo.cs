using System.Collections.Generic;

namespace DocumentDBConsoleDemo.Models
{
    public class Hijo
    {
        public string NombreFamilia { get; set; }
        public string Nombre { get; set; }
        public string SexoGenero { get; set; }
        public int Edad { get; set; }
        public List<Mascota> Mascota { get; set; }
    }
}