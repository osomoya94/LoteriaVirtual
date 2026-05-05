using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Entidades
{
    public class Sorteo
    {
            public int Id { get; set; }
            public string Nombre { get; set; } = string.Empty;
            public DateTime Fecha { get; set; }
            public decimal Precio { get; set; }
            public string ModoExtraccion { get; set; } = string.Empty;

}
}
