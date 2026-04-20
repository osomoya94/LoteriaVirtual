using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.Identity
{
    public class Usuario
    {
        public int Id { get; set; }
        public int RolId { get; set; }
        public required string Username { get; set; }
        public required string PasswordHash { get; set; }
        public bool Activo { get; set; } = true;
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public Rol? Rol { get; set; }
    }
}
