using Loteria.Datos.Repositorios;
using Loteria.Entidades.DTOs;
using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Loteria.Negocio.Servicios
{
    public class CartonService
    {
        private readonly CartonRepository _cartonRepository;
        private readonly SorteoRepository _sorteoRepository; // Agregamos la nueva herramienta
        private readonly JugadorRepository _jugadorRepository; 


        public CartonService(CartonRepository cartonRepository, SorteoRepository sorteoRepository, JugadorRepository jugadorRepository)
        {
            _cartonRepository = cartonRepository;
            _sorteoRepository = sorteoRepository;
            _jugadorRepository = jugadorRepository;
        }

        public async Task<IEnumerable<Carton>> ObtenerCartonesPorSorteoAsync(int idSorteo)
        {
            var sorteo = await _sorteoRepository.ObtenerPorIdAsync(idSorteo);

            if (sorteo == null)
            {
                throw new Exception($"No existe el sorteo número {idSorteo} en el sistema.");
            }

            return await _cartonRepository.ObtenerCartonesPorSorteoAsync(idSorteo);
        }

        public async Task CrearCartonAsync(Carton nuevoCarton) 
        {
            var sorteo = await _sorteoRepository.ObtenerPorIdAsync(nuevoCarton.Id_sorteo);
            if (sorteo == null)
            {
                throw new Exception($"No existe el sorteo número {nuevoCarton.Id_sorteo} en el sistema.");
            }

            await _cartonRepository.CrearCartonAsync(nuevoCarton);
        }

        public async Task ComprarCartonesAsync(CompraCartonesDTO pedido) 
        {
            if (pedido.CartonesIds == null || pedido.CartonesIds.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos un cartón.");
            }

            var jugador = await _jugadorRepository.ObtenerPorIdAsync(pedido.JugadorId);
            if (jugador == null) 
            {
                throw new Exception("Jugador no existe, no puede serservar cartones");
            }

            var filasAfectadas = await _cartonRepository.ReservarCartonesAsync(pedido.JugadorId, pedido.CartonesIds);

            if (filasAfectadas != pedido.CartonesIds.Count) 
            {
                throw new Exception("Error: Algunos cartones ya fueron vendidos o no están disponibles");
            }
        }

        public async Task AprobacionCartonPagoAsync(CompraCartonesDTO pedido) 
        {
            if (pedido.CartonesIds == null || pedido.CartonesIds.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos un cartón.");
            }

            var jugador = await _jugadorRepository.ObtenerPorIdAsync(pedido.JugadorId);
            if (jugador == null)
            {
                throw new Exception("Jugador no existe, no puede serservar cartones");
            }

            var filasAfectadas = await _cartonRepository.AprobacionCartonPagoAsync(pedido.JugadorId, pedido.CartonesIds);

            if (filasAfectadas != pedido.CartonesIds.Count)
            {
                throw new Exception("Error: algunos cartones no estaban reservados por este jugador.");
            }
        }

        public async Task CancelarCartonReservaAsync(CompraCartonesDTO pedido)
        {
            if (pedido.CartonesIds == null || pedido.CartonesIds.Count == 0)
            {
                throw new Exception("Debe seleccionar al menos un cartón.");
            }

            var jugador = await _jugadorRepository.ObtenerPorIdAsync(pedido.JugadorId);
            if (jugador == null)
            {
                throw new Exception("Jugador no existe");
            }

            var filasAfectadas = await _cartonRepository.CancelarCartonReservaAsync(pedido.JugadorId, pedido.CartonesIds);

            if (filasAfectadas != pedido.CartonesIds.Count)
            {
                throw new Exception("Error: algunos cartones no estaban reservados por este jugador.");
            }
        }

    }
}

