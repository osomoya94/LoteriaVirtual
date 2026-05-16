using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Loteria.Entidades.Identity;

namespace Loteria.Datos.Repositorios
{
    public class SorteoRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public SorteoRepository(ConnectionFactory connectionFactory) 
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Sorteo>> ObtenerTodosAsync()
        {
            string sql = "SELECT * FROM Sorteos;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryAsync<Sorteo>(sql);
            }
        }

        public async Task CrearSorteoAsync(Sorteo nuevoSorteo) 
        {
            string sql = @"INSERT INTO Sorteos (Nombre,Fecha_sorteo,Cantidad_cartones,Precio_carton,Porcentaje_premio,Intervalo_extraccion_segundos) VALUES  (@Nombre,@Fecha_sorteo,@Cantidad_cartones,@Precio_carton,@Porcentaje_premio,@Intervalo_extraccion_segundos)";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                await conexion.ExecuteAsync(sql,nuevoSorteo);
            }
        }

        //OBTENER EL ULTIMO SORTEO
        public async Task<Sorteo?> ObtenerUltimoSorteoAsync()
        {
            string sql = "SELECT * FROM Sorteos ORDER BY Fecha_sorteo DESC LIMIT 1;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                return await conexion.QueryFirstOrDefaultAsync<Sorteo>(sql);
            }
        }

        public async Task<Sorteo?> ObtenerPorIdAsync(int id) 
        {
            string sql = "SELECT * FROM Sorteos WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                return await conexion.QueryFirstOrDefaultAsync<Sorteo>(sql, new { Id = id });
            }
        }

        public async Task ActualizarSorteoAsync(Sorteo sorteoEditado) 
        {
            
            string sql = "UPDATE Sorteos SET Nombre = @Nombre, Fecha_sorteo = @Fecha_sorteo, Estado = @Estado, Porcentaje_premio = @Porcentaje_premio, Modo_extraccion = @Modo_extraccion, Permite_multiples_ganadores = @Permite_multiples_ganadores , Configuracion_premio = @Configuracion_premio  WHERE Id = @Id";

            using (var conexion = _connectionFactory.CreateConnection()) 
            {
                await conexion.ExecuteAsync(sql, sorteoEditado);
            }
        }

        public async Task CancelarSorteoAsync(int id)
        {
            string sql = "UPDATE Sorteos SET Estado = 'CANCELADO' WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, new { Id = id });
            }
        }

        public async Task ActualizarEstadoAsync(int id, string estado)
        {
            string sql = "UPDATE Sorteos SET Estado = @Estado WHERE Id = @Id;";

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, new { Id = id, Estado = estado });
            }
        }

        public async Task GuardarResultadoSorteoAsync(int idSorteo, string estado, List<int> listaNumeros, int idCartonGanador, int idJugadorGanador)
        {
            using (var conexion = _connectionFactory.CreateConnection())
            {
                conexion.Open(); // ¡Obligatorio abrir la conexión antes de iniciar la transacción!
                using (var transaccion = conexion.BeginTransaction())
                {
                    try
                    {
                        // 1. Actualizamos el estado del sorteo
                        string sqlSorteo = "UPDATE sorteos SET estado = @Estado WHERE id = @Id;";
                        // Notá que ahora le pasamos "transaccion" como tercer parámetro
                        await conexion.ExecuteAsync(sqlSorteo, new { Id = idSorteo, Estado = estado }, transaccion);

                        // 2. Guardamos las bolillas
                        var listaExtracciones = listaNumeros.Select((numero, indice) => new
                        {
                            id_sorteo = idSorteo,
                            numero_extraido = numero,
                            orden = indice + 1
                        }).ToList();

                        string sqlExtracciones = "INSERT INTO extracciones (id_sorteo, numero_extraido, orden) VALUES (@id_sorteo, @numero_extraido, @orden);";
                        await conexion.ExecuteAsync(sqlExtracciones, listaExtracciones, transaccion);

                        // 3. Guardamos el ganador (Le sacamos el COALESCE inventado para que busque el real)
                        string sqlGanador = @"
                INSERT INTO ganadores (id_sorteo, id_jugador, id_carton, id_asignacion, id_premio, criterio_ganador, confirmado) 
                VALUES (
                    @IdSorteo, 
                    @IdJugador, 
                    @IdCarton, 
                    (SELECT id FROM asignaciones_carton WHERE id_carton = @IdCarton LIMIT 1), 
                    COALESCE((SELECT id FROM premios WHERE id_sorteo = @IdSorteo LIMIT 1), 1), 
                    'Cartón Lleno', 
                    1
                );";

                        await conexion.ExecuteAsync(sqlGanador, new
                        {
                            IdSorteo = idSorteo,
                            IdJugador = idJugadorGanador,
                            IdCarton = idCartonGanador
                        }, transaccion);

                        // Si llegó hasta acá sin errores, guardamos todo definitivamente
                        transaccion.Commit();
                    }
                    catch (Exception ex)
                    {
                        // Si algo explota (ej: no encuentra la asignación), deshacemos los pasos 1 y 2
                        transaccion.Rollback();
                        throw new Exception("Error al guardar en la BD. Se canceló la operación para proteger los datos. Detalle: " + ex.Message);
                    }
                }
            }
        }



    }
}
