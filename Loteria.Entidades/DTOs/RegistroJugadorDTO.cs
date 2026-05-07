using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class RegistroJugadorDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
        public required string Nombre { get; set; }
        public required string Apellido { get; set; }
        public required string Dni { get; set; }
        public required string Email{ get; set; }
    }
}
