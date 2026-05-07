using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class CompraCartonesDTO
    {
        public required int JugadorId { get; set; }
        public required List<int> CartonesIds { get; set; }
    }
}
