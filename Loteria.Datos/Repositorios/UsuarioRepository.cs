using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper; // CRÍTICO: La herramienta mágica
using System.Data;
using Loteria.Entidades.Identity; // Para que sepa qué es un "Usuario"

namespace Loteria.Datos.Repositorios
{
    public class UsuarioRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public UsuarioRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        // Método asíncrono para obtener todos los usuarios
        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync()
        {
            // 1. La consulta SQL que vos mismo dedujiste
            string sql = "SELECT * FROM Usuarios;";

            // 2. Usamos la fábrica para crear la conexión. 
            // El bloque 'using' asegura que la conexión se cierre sola al terminar, ¡es vital para no saturar XAMPP!
            using (var conexion = _connectionFactory.CreateConnection())
            {
                // 3. Dapper ejecuta el SQL y mapea las columnas a las propiedades de la clase Usuario
                return await conexion.QueryAsync<Usuario>(sql);
            }
        }


        // Método para buscar un usuario específico por su ID
        public async Task<Usuario?> ObtenerPorIdAsync(int id)
        {
            // Fíjate en el @Id, así nos protegemos de los hackeos
            string sql = "SELECT * FROM Usuarios WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                // QueryFirstOrDefaultAsync devuelve el usuario si lo encuentra, o "null" si no existe.
                // Además, le pasamos la variable "id" empaquetada para reemplazar el @Id.
                return await conexion.QueryFirstOrDefaultAsync<Usuario>(sql, new { Id = id });
            }
        }

        // Método para registrar un nuevo usuario en el sistema
        public async Task CrearUsuarioAsync(Usuario nuevoUsuario)
        {
            // Usamos los nombres exactos de las columnas de tu script SQL
            string sql = @"INSERT INTO usuarios (RolId, Username, PasswordHash, Activo) 
                           VALUES (@RolId, @Username, @PasswordHash, @Activo);";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                // ExecuteAsync es el verbo correcto para INSERT, UPDATE o DELETE
                await conexion.ExecuteAsync(sql, nuevoUsuario);
            }
        }

        public async Task ActualizarUsuarioAsync(Usuario usuarioEditado) 
        {
            string sql = "UPDATE Usuarios SET Username = @Username , PasswordHash = @PasswordHash WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                await conexion.ExecuteAsync(sql, usuarioEditado);
            }
        }

        public async Task EliminarUsuarioAsync(int id) 
        {
            // Aseguramos que el parámetro coincida con @Id y quitamos el genérico incorrecto de ExecuteAsync.
            string sql = "UPDATE Usuarios SET Activo = false WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, new { Id = id });
            }
        }

        // buscar que no se repita username
        public async Task<Usuario?> ObtenerPorUsernameAsync(string username) 
        {
            string sql = "SELECT * FROM Usuarios WHERE Username = @Username;";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                return await conexion.QueryFirstOrDefaultAsync<Usuario>(sql, new { Username = username });
            }
        }

    }
}


