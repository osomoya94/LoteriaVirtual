using Dapper;

namespace Loteria.Datos.Repositorios
{
    public class ExtraccionRepository
    {
        private readonly ConnectionFactory _connectionFactory;

        public ExtraccionRepository(ConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task InsertarMasivoAsync(int idSorteo, List<int> listaNumeros)
        {
            if (listaNumeros.Count == 0)
            {
                return;
            }

            string sql = @"
                INSERT INTO extracciones (id_sorteo, numero_extraido, orden)
                VALUES (@IdSorteo, @NumeroExtraido, @Orden);";

            var extracciones = listaNumeros.Select((numero, indice) => new
            {
                IdSorteo = idSorteo,
                NumeroExtraido = numero,
                Orden = indice + 1
            });

            using (var conexion = _connectionFactory.CreateConnection())
            {
                await conexion.ExecuteAsync(sql, extracciones);
            }
        }
    }
}
