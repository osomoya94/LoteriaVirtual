using System.Collections.Generic;

namespace Loteria.Entidades.DTOs
{
    public class ResultadoSorteoConsultaDTO
    {
        public int IdSorteo { get; set; }
        public string Estado { get; set; } = "";
        public List<NumeroExtraidoDTO> NumerosExtraidos { get; set; } = new List<NumeroExtraidoDTO>();
        public List<CartonGanadorResultadoDTO> CartonesGanadores { get; set; } = new List<CartonGanadorResultadoDTO>();
    }

    public class NumeroExtraidoDTO
    {
        public int Orden { get; set; }
        public int NumeroExtraido { get; set; }
    }

    public class CartonGanadorResultadoDTO
    {
        public int CartonId { get; set; }
        public string CodigoUnico { get; set; } = "";
        public string PatronContenido { get; set; } = "";
        public int JugadorId { get; set; }
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Dni { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
