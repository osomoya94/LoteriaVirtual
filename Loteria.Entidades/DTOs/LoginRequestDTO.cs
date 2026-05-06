using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class LoginRequestDTO
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }
}
