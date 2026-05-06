using BCrypt.Net;
using Loteria.Datos.Repositorios;
using Loteria.Entidades.DTOs;
using Loteria.Entidades.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt; // Para JwtSecurityTokenHandler
using Microsoft.IdentityModel.Tokens;    // Para SecurityTokenDescriptor



namespace Loteria.Negocio.Servicios
{ 
    public class UsuarioService
    {
        private readonly UsuarioRepository _usuarioRepository;
        private readonly JugadorRepository _jugadorRepository;
        private readonly IConfiguration configuration;



        public UsuarioService(UsuarioRepository usuarioRepository , JugadorRepository jugadorRepository, IConfiguration configuration)
        {
            _usuarioRepository = usuarioRepository;
            _jugadorRepository = jugadorRepository;
            this.configuration = configuration;
        }


        public async Task<IEnumerable<UsuarioResponseDTO>> ObtenerTodosAsync()
        {
            var usuarioTodos = await _usuarioRepository.ObtenerTodosAsync();

            if (!usuarioTodos.Any())
            {
                throw new Exception("No hay usuarios registrados"); 
            }

            var usuariosSinHash = usuarioTodos.Select(u => new UsuarioResponseDTO
            {
                Id = u.Id,
                RolId = u.RolId,
                Username = u.Username,
                Activo = u.Activo
            }).ToList();

            return usuariosSinHash;
        }

        public async Task<UsuarioResponseDTO?> ObtenerPorIdAsync(int id) 
        {
            var usuarioSolo = await _usuarioRepository.ObtenerPorIdAsync(id);

            if (usuarioSolo == null) 
            {
                throw new Exception($"No existe el usuario de  id: {id}");
            }

            return new UsuarioResponseDTO 
            {
                Id=id,
                RolId =usuarioSolo.RolId,
                Username=usuarioSolo.Username,
                Activo=usuarioSolo.Activo
            };
        }


        public async Task CrearUsuarioAsync(Usuario nuevoUsuario)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(nuevoUsuario.Username);

            if (usuarioExiste != null)
            {
                throw new Exception("El Username ya está registrado por otro usuario en el sistema.");
            }

            string hashSeguro = BCrypt.Net.BCrypt.HashPassword(nuevoUsuario.PasswordHash);

            nuevoUsuario.PasswordHash = hashSeguro;

            await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario);
        }

        public async Task RegistrarJugadorWebAsync(RegistroJugadorDTO request)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(request.Username);

            if (usuarioExiste != null)
            {
                throw new Exception("El Username ya está registrado por otro usuario en el sistema.");
            }

            var jugadorExiste = await _jugadorRepository.ObtenerPorDniOEmailAsync(request.Dni, request.Email);

            if (jugadorExiste != null)
            {
                throw new Exception("El DNI o el Email ya están registrados en el sistema.");
            }

            string hashSeguro = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var nuevoUsuario = new Usuario
            {
                RolId = 2,// tengamos en cuenta que no creamos los roles aun, podriamos hacer una funcion que los cree por primera vez en la base de datos y ya lo tengamos definidos. ya que si no lo tenemos creado nos dara error, osea que se cren por defecto cuando levante la base de datosy si existen que no haga nada
                Username = request.Username,
                PasswordHash = hashSeguro,
                Activo = true
            };

       
            int nuevoUsuarioId = await _usuarioRepository.CrearUsuarioAsync(nuevoUsuario);

            var nuevoJugador = new Jugador
            {
                UsuarioId = nuevoUsuarioId,
                Nombre = request.Nombre,
                Apellido = request.Apellido,
                Dni = request.Dni,
                Email = request.Email,
                Saldo = 0 
            };
            
            await _jugadorRepository.CrearJugadorAsync(nuevoJugador);
        }



        public async Task ActualizarUsuarioAsync(Usuario usuarioEditado)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(usuarioEditado.Username);

            //  Verificamos: ¿Existe alguien con ese nombre? Y si existe... ¿es una persona DISTINTA?
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


        public async Task<UsuarioResponseLoginDTO> LoginAsync(LoginRequestDTO request)
        {
            var usuarioExiste = await _usuarioRepository.ObtenerPorUsernameAsync(request.Username);

            if (usuarioExiste == null)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }

            if (!usuarioExiste.Activo)
            {
                throw new Exception("El usuario está inactivo.");
            }

            bool verificar = BCrypt.Net.BCrypt.Verify(request.Password, usuarioExiste.PasswordHash);

            if (!verificar)
            {
                throw new Exception("Usuario o contraseña incorrectos");
            }

            // 1. Traemos la clave y los datos del appsettings.json
            var secretKey = configuration["Jwt:Key"];
            var keyBytes = Encoding.UTF8.GetBytes(secretKey);

            // 2. Creamos los "Claims" (los datos que viajan adentro del pasaporte)
            var claims = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuarioExiste.Id.ToString()),
                new Claim(ClaimTypes.Role, usuarioExiste.RolId.ToString()),
                new Claim(ClaimTypes.Name, usuarioExiste.Username)
            });

            // 3. Configuramos el pasaporte (vencimiento y firma criptográfica)
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(1), // El pasaporte dura 1 horas
                Issuer = configuration["Jwt:Issuer"],
                Audience = configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            // 4. Fabricamos el string final del token
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string tokenFinal = tokenHandler.WriteToken(tokenConfig);

            return new UsuarioResponseLoginDTO
            {
                Id = usuarioExiste.Id,
                RolId = usuarioExiste.RolId,
                Username = usuarioExiste.Username,
                Activo = usuarioExiste.Activo,
                Token = tokenFinal,
            };
        }


    }
}
