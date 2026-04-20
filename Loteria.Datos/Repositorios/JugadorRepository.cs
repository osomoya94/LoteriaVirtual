using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Loteria.Entidades.Identity;

namespace Loteria.Datos.Repositorios
{
    public class JugadorRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public JugadorRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        //Verificar si existe Email y Dni
        public async Task<Jugador?> ObtenerPorDniOEmailAsync(string dni, string email) 
        {
            string sql = "SELECT * FROM Jugadores WHERE Dni=@Dni OR Email=@Email;";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                return await conexion.QueryFirstOrDefaultAsync<Jugador>(sql , new { Dni = dni, Email = email });
            }
        }

        //Crear jugador
        public async Task CrearJugadorAsync(Jugador nuevoJugador) 
        {
            string sql = @"INSERT INTO jugadores (UsuarioId, Dni, Nombre, Apellido, Email, Estado) 
               VALUES (@UsuarioId, @Dni, @Nombre, @Apellido, @Email, @Estado);";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                await conexion.ExecuteAsync(sql,nuevoJugador);
            }
        }

        // obtener todos los jugadores
        public async Task<IEnumerable<Jugador>> ObtenerTodosAsync()
        {
            string sql = "SELECT * FROM Jugadores WHERE Estado = 'Activo';";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryAsync<Jugador>(sql);
            }
        }

        // obtener un jugador
        public async Task<Jugador?> ObtenerPorIdAsync(int id)
        {
            string sql = "SELECT * FROM Jugadores WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryFirstOrDefaultAsync<Jugador>(sql, new { Id = id });
            }
        }

        //Actulizar jugador
        public async Task ActualizarJugadorAsync(Jugador jugadorEditado)
        {
            string sql = "UPDATE Jugadores SET Email = @Email WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, jugadorEditado);
            }
        }

        // buscar que no se repita el email
        public async Task<Jugador?> ObtenerPorEmailAsync(string email)
        {
            string sql = "SELECT * FROM Jugadores WHERE Email = @Email;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryFirstOrDefaultAsync<Jugador>(sql, new { Email = email });
            }
        }

        //Elimina jugador
        public async Task EliminarJugadorAsync(int id)
        {
            string sql = "UPDATE Jugadores SET Estado = 'BAJA' WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, new { Id = id });
            }
        }


    }
}
