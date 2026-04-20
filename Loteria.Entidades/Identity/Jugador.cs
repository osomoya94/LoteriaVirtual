using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.Identity
{
    public class Jugador
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public required string Dni { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Email { get; set; }
        public decimal Saldo { get; set; }
        public string Estado { get; set; } = "Activo";
    }
}
