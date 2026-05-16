using Loteria.Datos.Repositorios;
using Loteria.Entidades.DTOs;
using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Loteria.Negocio.Servicios
{
    public class SorteoService
    {
        private const string EstadoBorrador = "BORRADOR";
        private const string EstadoEnVenta = "ABIERTO";
        private const string EstadoFinalizado = "FINALIZADO";

        private readonly SorteoRepository _sorteoRepository;
        private readonly CartonRepository _cartonRepository;

        public SorteoService(SorteoRepository sorteoRepository, CartonRepository cartonRepository)
        {
            _sorteoRepository = sorteoRepository;
            _cartonRepository = cartonRepository;
        }

        public async Task<IEnumerable<Sorteo>> ObtenerTodosAsync() 
        {
            return await _sorteoRepository.ObtenerTodosAsync();
        }

        public async Task CrearSorteoAsync(Sorteo nuevoSorteo)
        {
            if (nuevoSorteo.Fecha_sorteo <= DateTime.Now)
            {
                throw new Exception("La fecha debe ser posterior a la fecha actual");
            }

            var ultimoSorteo = await _sorteoRepository.ObtenerUltimoSorteoAsync();

            if (ultimoSorteo != null)
            {
                double diferenciaDias = (nuevoSorteo.Fecha_sorteo - ultimoSorteo.Fecha_sorteo).TotalDays;

                if (Math.Abs(diferenciaDias) < 2) 
                {
                    throw new Exception("Para realizar un proximo sorteo deben pasar dos dias minimo");
                }

            }

            await _sorteoRepository.CrearSorteoAsync(nuevoSorteo);
        }

        public async Task<Sorteo?> ObtenerPorIdAsync(int id) 
        {
            return await _sorteoRepository.ObtenerPorIdAsync(id);
        }

        public async Task ActualizarSorteoAsync(Sorteo sorteoEditado) 
        {
            if (sorteoEditado.Fecha_sorteo <= DateTime.Now)
            {
                throw new Exception("La fecha debe ser posterior a la fecha actual");
            }

            var ultimoSorteo = await _sorteoRepository.ObtenerUltimoSorteoAsync();

            if (ultimoSorteo != null && ultimoSorteo.Id != sorteoEditado.Id ) 
            {
                double diferenciaDias = (sorteoEditado.Fecha_sorteo - ultimoSorteo.Fecha_sorteo).TotalDays;

                if (Math.Abs(diferenciaDias) < 2) 
                {
                    throw new Exception("Para modificar la dia del sorte tiene que tene dos dias de diferencia minimo");
                }
            }

            await _sorteoRepository.ActualizarSorteoAsync(sorteoEditado);
        }

        public async Task CancelarSorteoAsync(int id)
        {
            var sorteo = await _sorteoRepository.ObtenerPorIdAsync(id);

            if (sorteo == null)
            {
                throw new Exception($"No existe el sorteo número {id}.");
            }


            if (sorteo.Estado == "FINALIZADO")
            {
                throw new Exception("No se puede cancelar un sorteo que ya está finalizado.");
            }

            await _sorteoRepository.CancelarSorteoAsync(id);
        }


        public async Task AbrirSorteoAsync(int id, int cantidadPedida ) 
        {
            var sorteo = await ObtenerPorIdAsync(id);

            if(sorteo == null || sorteo.Estado != EstadoBorrador) 
            {
                throw new Exception ("El sorteo no existe o ya fue realizado");
            }

            if (cantidadPedida <= 0)
            {
                throw new Exception("La cantidad de cartones a generar debe ser mayor a cero.");
            }

            string rutaCartones = Path.Combine(AppContext.BaseDirectory, "Recursos", "Cartones.txt");
            if (!File.Exists(rutaCartones))
            {
                throw new FileNotFoundException("No se encontró el archivo de patrones de cartones.", rutaCartones);
            }

            string[] todosLosRenglones = await File.ReadAllLinesAsync(rutaCartones);

            if (cantidadPedida > todosLosRenglones.Length)
            {
                throw new Exception($"No hay suficientes patrones en Cartones.txt. Pedidos: {cantidadPedida}, disponibles: {todosLosRenglones.Length}.");
            }

            
            var renglonesElegidos = todosLosRenglones
                .OrderBy(renglon => Guid.NewGuid()) 
                .Take(cantidadPedida) 
                .ToList();

            var nuevosCartones = new List<Carton>();
            
            foreach (var renglon in renglonesElegidos)
            {
                string[] pedacitos = renglon.Split(',');

                var quinceNumeros = pedacitos.Skip(1).Take(15);

                string patron = string.Join("-", quinceNumeros);

                string numeroCartonArchivo = pedacitos[0].Trim();

                string codigoOriginal = pedacitos.Last().Replace("\"", "").Trim();

                var nuevoCarton = new Carton
                {
                    Id_sorteo = sorteo.Id,

                    Codigo_unico = $"{codigoOriginal}-{numeroCartonArchivo}-{sorteo.Id}",

                    Patron_contenido = patron,

                    Hash_contenido = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(patron))),

                    Estado = "DISPONIBLE",
                    fecha_generacion = DateTime.UtcNow 
                };

                nuevosCartones.Add(nuevoCarton);
            }

            await _cartonRepository.CrearCartonesMasivoAsync(nuevosCartones);
            await _sorteoRepository.ActualizarEstadoAsync(sorteo.Id, EstadoEnVenta);

        }


        public async Task<ResultadoSorteoDTO> RealizarSorteoAsync(int idSorteo) 
        {

            var sorteoActual = await _sorteoRepository.ObtenerPorIdAsync(idSorteo);

            if (sorteoActual == null)
            {
                throw new Exception("El sorteo no existe.");
            }
            if (sorteoActual.Estado.ToUpper() != "ABIERTO")
            {
                throw new Exception($"No se puede jugar. El sorteo se encuentra en estado: {sorteoActual.Estado}");
            }


            var cartonesVendidos = await _cartonRepository.ObtenerCartonesVendidosPorSorteoAsync(idSorteo);

            if (cartonesVendidos == null || !cartonesVendidos.Any() ) 
            {
                throw new Exception("No tenemos cartones vendidos para realizar el sorteo ");
            }

            var bolillero = Enumerable.Range(1, 90).OrderBy(x => Guid.NewGuid()).ToList();

            List<int> listaNumeros = new List<int>();
            bool hayGanador = false;
            int i = 0;
            List <Carton> cartonGanador = new List<Carton>();

            while (!hayGanador) 
            {
                int numero = bolillero[i];

                listaNumeros.Add(numero);

                foreach (Carton carton in cartonesVendidos ) 
                {
                    var numerosDelCarton = carton.Patron_contenido.Split('-').Select(int.Parse).ToList();

                    bool esGanador = numerosDelCarton.All(n => listaNumeros.Contains(n));

                    if (esGanador) 
                    {
                        cartonGanador.Add(carton);
                        hayGanador = true;
                    }
                }

                i++;
            }

            var primerGanador = cartonGanador.FirstOrDefault();
            int idGanador = primerGanador != null ? primerGanador.Id : 0;
            int idJugador = primerGanador != null ? (primerGanador.JugadorId ?? 0) : 0;

            await _sorteoRepository.GuardarResultadoSorteoAsync(idSorteo, EstadoFinalizado, listaNumeros, idGanador, idJugador);

            var resultado = new ResultadoSorteoDTO
            {
                ListaNumeros = listaNumeros,
                CartonGanadores = cartonGanador
            };

            return resultado;
        }




    }

}

