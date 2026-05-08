using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades.Identity
{ public class Pago
        {
            public int Id { get; set; }
            public string DniJugador { get; set; } = string.Empty;
            public decimal Monto { get; set; }
            public DateTime Fecha { get; set; }
            public string Estado { get; set; } = "Pendiente"; // Valor por defecto
      
    }
}
