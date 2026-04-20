using Loteria.Datos.Repositorios;
using Loteria.Entidades.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Negocio.Servicios
{
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;

        public UsuarioService(UsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync() 
        {
            return await _usuarioRepository.ObtenerTodosAsync();
        }

        public async Task<Usuario?> ObtenerPorIdAsync(int id) 
        {
            return await _usuarioRepository.ObtenerPorIdAsync(id);
        }

        public async Task CrearUsuarioAsync(Usuario nuevoUsuario)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(nuevoUsuario.Username);

            // 2. Verificamos: ¿Existe alguien con ese nombre? Y si existe... ¿es una persona DISTINTA?
            if (usuarioExiste != null)
            {
                throw new Exception("El Username ya está registrado por otro usuario en el sistema.");
            }

            await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario);
        }

        public async Task ActualizarUsuarioAsync(Usuario usuarioEditado)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(usuarioEditado.Username);

            // 2. Verificamos: ¿Existe alguien con ese nombre? Y si existe... ¿es una persona DISTINTA?
            if (usuarioExiste != null && usuarioExiste.Id != usuarioEditado.Id)
            {
                throw new Exception("El Username ya está registrado por otro usuario en el sistema.");
            }

            await _usuarioRepository.ActualizarUsuarioAsync(usuarioEditado);
        }

        public async Task EliminarUsuarioAsync(int id) 
        {
            await _usuarioRepository.EliminarUsuarioAsync(id);
        }
    }
}
