using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.DTOs
{
    public class ResultadoSorteoDTO
    {
        public required List<int> ListaNumeros { get; set; }
        public required List<Carton> CartonGanadores { get; set; }
    }
}
