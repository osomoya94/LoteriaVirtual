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
            
            string sql = "UPDATE Sorteos SET Nombre = @Nombre, Fecha_sorteo = @Fecha_sorteo, Estado = @Estado, Porcentaje_premio = @Porcentaje_premio, Modo_extraccion = @Modo_extraccion WHERE Id = @Id";

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

    }
}
