using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class ApiResponseDTO
    {
        public bool OK { get; set; }
        public  string? Mensaje { get; set; }
        public object? Data { get; set; }
        public object? Errores { get; set; }
    }
}
