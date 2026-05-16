using Dapper;
using Loteria.Datos.Repositorios;
using Loteria.Entidades.DTOs;
using Loteria.Entidades.Identity;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loteria.Datos.Repositorios
{
    public class CartonRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public CartonRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Carton>> ObtenerCartonesPorSorteoAsync(int idSorteo)
        {
            string sql = "SELECT * FROM Cartones WHERE id_sorteo = @Id_sorteo;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryAsync<Carton>(sql, new { Id_sorteo = idSorteo });
            }
        }

      
        public async Task CrearCartonAsync(Carton nuevoCarton) 
        {
            string sql = "INSERT INTO Cartones (Id_sorteo,Codigo_unico,Patron_contenido,Hash_contenido) VALUES (@Id_sorteo,@Codigo_unico,@Patron_contenido,@Hash_contenido)";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                await conexion.ExecuteAsync(sql,nuevoCarton);
            }
        }

        public async Task CrearCartonesMasivoAsync(List<Carton> cartones)
        {
            string sql = @"INSERT INTO Cartones (Id_sorteo, Codigo_unico, Patron_contenido, Hash_contenido, Estado, fecha_generacion)
                   VALUES (@Id_sorteo, @Codigo_unico, @Patron_contenido, @Hash_contenido, @Estado, @fecha_generacion);";

            // 1. Le avisamos que es una conexión de MySQL para que nos deje usar métodos Async
            using (var conexion = (MySqlConnection)_connectionFactory.CreateConnection())
            {
                await conexion.OpenAsync();
                using (var transaccion = await conexion.BeginTransactionAsync())
                {
                    try
                    {
                        // 2. Le pasamos la 'transaccion' a Dapper como tercer parámetro
                        await conexion.ExecuteAsync(sql, cartones, transaction: transaccion);

                        // Si todo sale perfecto, confirmamos y guardamos en la base
                        await transaccion.CommitAsync();
                    }
                    catch (Exception)
                    {
                        // Si falla un solo cartón, cancelamos todo el bloque
                        await transaccion.RollbackAsync();
                        throw;
                    }
                }
            }
        }

        public async Task<int> ReservarCartonesAsync(int jugadorId, List<int> cartonesIds)
        {
            string sql = @"UPDATE cartones 
                   SET estado = 'RESERVADO', jugador_id = @JugadorId 
                   WHERE id IN @CartonesIds AND estado = 'DISPONIBLE';";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.ExecuteAsync(sql, new { JugadorId = jugadorId, CartonesIds = cartonesIds });
            }
        }

        //Aprobaciones o Rechazos del pago
        public async Task<int> AprobacionCartonPagoAsync(int jugadorId, List<int> cartonesIds) 
        {
            string sql = @"UPDATE cartones 
                   SET estado = 'VENDIDO', jugador_id = @JugadorId 
                   WHERE id IN @CartonesIds AND estado = 'RESERVADO' AND jugador_id = @JugadorId;";
            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.ExecuteAsync(sql, new { JugadorId = jugadorId, CartonesIds = cartonesIds });
            }
        }

        public async Task<int> CancelarCartonReservaAsync(int jugadorId, List<int> cartonesIds)
        {
            string sql = @"UPDATE cartones 
               SET estado = 'DISPONIBLE', jugador_id = NULL 
               WHERE id IN @CartonesIds AND estado = 'RESERVADO' AND jugador_id = @JugadorId;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.ExecuteAsync(sql, new { JugadorId = jugadorId, CartonesIds = cartonesIds });
            }
        }

        public async Task<IEnumerable<Carton>> ObtenerCartonesVendidosPorSorteoAsync(int idSorteo) 
        {
            string sql = "SELECT id, id_sorteo, jugador_id AS JugadorId, codigo_unico, patron_contenido, hash_contenido, estado, fecha_generacion FROM cartones WHERE estado = 'VENDIDO' AND Id_sorteo = @idSorteo";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                return await conexion.QueryAsync<Carton>(sql, new { idSorteo = idSorteo });
            }
        }

        public async Task<IEnumerable<Carton>> ObtenerCartonesPorJugadorAsync(int jugadorId)
        {
            string sql = "SELECT id, id_sorteo, jugador_id AS JugadorId, codigo_unico, patron_contenido, hash_contenido, estado, fecha_generacion FROM cartones WHERE jugador_id = @jugadorId";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryAsync<Carton>(sql, new { jugadorId = jugadorId });
            }
        }

        public async Task<IEnumerable<CartonMisJugadasDTO>> ObtenerMisJugadasAsync(int jugadorId)
        {
            string sql = @"
                SELECT
                    c.id AS Id,
                    c.codigo_unico AS CodigoUnico,
                    c.patron_contenido AS PatronContenido,
                    c.estado AS Estado,
                    s.nombre AS SorteoNombre,
                    s.fecha_sorteo AS SorteoFecha
                FROM cartones c
                INNER JOIN sorteos s ON s.id = c.id_sorteo
                WHERE c.jugador_id = @JugadorId
                ORDER BY s.fecha_sorteo DESC, c.id DESC;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryAsync<CartonMisJugadasDTO>(sql, new { JugadorId = jugadorId });
            }
        }
    }
}
