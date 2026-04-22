using Loteria.Datos.Repositorios;
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

        public CartonService(CartonRepository cartonRepository, SorteoRepository sorteoRepository)
        {
            _cartonRepository = cartonRepository;
            _sorteoRepository = sorteoRepository;
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

        
    }
}

