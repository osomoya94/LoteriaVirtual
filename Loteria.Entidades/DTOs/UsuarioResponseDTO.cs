using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public required string Username { get; set; }
        public bool Activo { get; set; } 
    }
}
