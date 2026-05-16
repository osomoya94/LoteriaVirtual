using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class UsuarioResponseLoginDTO
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public required string Username { get; set; }
        public bool Activo { get; set; }
        public required string Token { get; set; }
        public int? JugadorId { get; set; }
    }
}
