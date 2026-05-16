using System;

namespace Loteria.Entidades.DTOs
{
    public class CartonMisJugadasDTO
    {
        public int Id { get; set; }
        public required string CodigoUnico { get; set; }
        public required string PatronContenido { get; set; }
        public required string Estado { get; set; }
        public required string SorteoNombre { get; set; }
        public DateTime SorteoFecha { get; set; }
    }
}
