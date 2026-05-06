using System;
using System.Collections.Generic;
using System.Text;
using Loteria.Datos.Repositorios;
using Loteria.Entidades.Identity;

namespace Loteria.Negocio.Servicios
{
    public class JugadorService
    {
        private readonly JugadorRepository _jugadorRepository;
        private readonly CartonRepository _cartonRepository;

        public JugadorService(JugadorRepository jugadorRepository, CartonRepository cartonRepository) 
        {
            _jugadorRepository = jugadorRepository;
            _cartonRepository = cartonRepository;
        }

        public async Task CrearJugadorAsync(Jugador nuevoJugador) 
        {
            var jugadorExiste = await _jugadorRepository.ObtenerPorDniOEmailAsync(nuevoJugador.Dni, nuevoJugador.Email);

            if (jugadorExiste != null) 
            {
                throw new Exception("El DNI o el Email ya están registrados en el sistema.");
            }

            await _jugadorRepository.CrearJugadorAsync(nuevoJugador);
        }

        public async Task<IEnumerable<Jugador>> ObtenerTodosAsync()
        {
            var jugadoresTodos = await _jugadorRepository.ObtenerTodosAsync();
            if (!jugadoresTodos.Any())
            {
                throw new Exception("No hay jugadores registrados");
            }

            return jugadoresTodos;
        }

        public async Task<Jugador?> ObtenerPorIdAsync(int id)
        {
            var jugadorSolo = await _jugadorRepository.ObtenerPorIdAsync(id);

            if (jugadorSolo == null) return null;

            var cartonesJugador = await _cartonRepository.ObtenerCartonesPorJugadorAsync(id);

            jugadorSolo.Cartones = cartonesJugador.ToList();

            return jugadorSolo;
        }

        public async Task ActualizarJugadorAsync(Jugador jugadorEditado)
        {
            var jugadorExiste = await _jugadorRepository.ObtenerPorDniOEmailAsync(jugadorEditado.Dni, jugadorEditado.Email);

            if (jugadorExiste != null && jugadorExiste.Id != jugadorEditado.Id)
            {
                throw new Exception("El DNI o el Email ya están registrados por otro jugador en el sistema.");
            }

            await _jugadorRepository.ActualizarJugadorAsync(jugadorEditado);
        }

        public async Task EliminarJugadorAsync(int id)
        {
            await _jugadorRepository.EliminarJugadorAsync(id);
        }

    }
}
