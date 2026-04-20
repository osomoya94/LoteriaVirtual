using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.Identity
{
    public class Rol
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public string? Descripcion { get; set; }
    }
}
