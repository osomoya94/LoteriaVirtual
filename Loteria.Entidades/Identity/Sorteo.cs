using System;

namespace Loteria.Entidades.Identity
{
    public class Sorteo
    {
        public int Id { get; set; }
        public required string Nombre { get; set; }
        public DateTime Fecha_sorteo { get; set; }
        public string Estado { get; set; } = "BORRADOR";
        public int Cantidad_cartones { get; set; }
        public decimal Precio_carton { get; set; }
        public decimal Porcentaje_premio { get; set; }

        public int Intervalo_extraccion_segundos { get; set; }
        public string Modo_extraccion { get; set; } = "AUTOMATICA";

        public bool Permite_multiples_ganadores { get; set; } = false;

        public string? Configuracion_premio { get; set; }
    }
}