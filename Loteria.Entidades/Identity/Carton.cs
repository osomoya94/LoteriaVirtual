using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.Identity
{
    public class Carton
    {
        public int Id { get; set; }
        public int Id_sorteo { get; set; }
        public required string Codigo_unico { get; set; }
        public required string Patron_contenido { get; set; }
        public required string Hash_contenido { get; set; }
        public string Estado { get; set; } = "DISPONIBLE";
        public DateTime fecha_generacion { get; set; }

    }
}
