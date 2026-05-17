using System;
using System.Collections.Generic;
using System.Text;
using Dapper;
using Loteria.Entidades.DTOs;
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

        public async Task<ResultadoSorteoConsultaDTO?> ObtenerResultadosAsync(int idSorteo)
        {
            using (var conexion = _connectionFactory.CreateConnection())
            {
                string sqlSorteo = @"
                    SELECT
                        id AS IdSorteo,
                        estado AS Estado
                    FROM sorteos
                    WHERE id = @IdSorteo;";

                var resultado = await conexion.QueryFirstOrDefaultAsync<ResultadoSorteoConsultaDTO>(sqlSorteo, new { IdSorteo = idSorteo });

                if (resultado == null)
                {
                    return null;
                }

                string sqlExtracciones = @"
                    SELECT
                        orden AS Orden,
                        numero_extraido AS NumeroExtraido
                    FROM extracciones
                    WHERE id_sorteo = @IdSorteo
                    ORDER BY orden;";

                var numeros = await conexion.QueryAsync<NumeroExtraidoDTO>(sqlExtracciones, new { IdSorteo = idSorteo });

                string sqlGanadores = @"
                    SELECT
                        c.id AS CartonId,
                        c.codigo_unico AS CodigoUnico,
                        c.patron_contenido AS PatronContenido,
                        j.Id AS JugadorId,
                        j.Nombre AS Nombre,
                        j.Apellido AS Apellido,
                        j.Dni AS Dni,
                        j.Email AS Email
                    FROM cartones c
                    INNER JOIN jugadores j ON j.Id = c.jugador_id
                    WHERE c.id_sorteo = @IdSorteo
                      AND c.estado = 'GANADOR'
                    ORDER BY c.id;";

                var ganadores = await conexion.QueryAsync<CartonGanadorResultadoDTO>(sqlGanadores, new { IdSorteo = idSorteo });

                resultado.NumerosExtraidos = numeros.ToList();
                resultado.CartonesGanadores = ganadores.ToList();

                return resultado;
            }
        }



    }
}
