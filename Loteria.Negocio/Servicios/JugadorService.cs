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

        public JugadorService(JugadorRepository jugadorRepository) 
        {
            _jugadorRepository = jugadorRepository;
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
            return await _jugadorRepository.ObtenerTodosAsync();
        }

        public async Task<Jugador?> ObtenerPorIdAsync(int id)
        {
            return await _jugadorRepository.ObtenerPorIdAsync(id);
        }

        public async Task ActualizarJugadorAsync(Jugador jugadorEditado)
        {
            var jugadorExiste = await _jugadorRepository.ObtenerPorEmailAsync(jugadorEditado.Email);

            if (jugadorExiste != null && jugadorExiste.Id != jugadorEditado.Id)
            {
                throw new Exception("El Jugador ya está registrado por otro email en el sistema.");
            }

            await _jugadorRepository.ActualizarJugadorAsync(jugadorEditado);
        }

        public async Task EliminarJugadorAsync(int id)
        {
            await _jugadorRepository.EliminarJugadorAsync(id);
        }

    }
}
