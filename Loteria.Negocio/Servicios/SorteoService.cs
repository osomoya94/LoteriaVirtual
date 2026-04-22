using Loteria.Datos.Repositorios;
using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Negocio.Servicios
{
    public class SorteoService
    {
        private readonly SorteoRepository _sorteoRepository;

        public SorteoService(SorteoRepository sorteoRepository)
        {
            _sorteoRepository = sorteoRepository;
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


    }
}

