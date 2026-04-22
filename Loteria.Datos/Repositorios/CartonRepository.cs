using Dapper;
using Loteria.Datos.Repositorios;
using Loteria.Entidades.Identity;
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


    }
}
