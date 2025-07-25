using API_Leitura_Arquivo.Model;
using System.Data.SQLite;
using System.Net.Http.Headers;
using System.Reflection;

namespace API_Leitura_Arquivo.Data
{
    public class DatabaseService
    {
        private static readonly string assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        private static readonly string rootPath = Directory.GetParent(assemblyPath)?.Parent?.Parent?.Parent?.FullName; // Subir dois níveis
        private static readonly string dbFolder = Path.Combine(rootPath ?? assemblyPath, "DataBase");
        private static readonly string dbFile = Path.Combine(dbFolder, "database.db");
        private static readonly string connectionString = $"Data Source={dbFile};Version=3;";

        public static List<Pedido_Produto> ObterTodosPedidos(int pedcod)
        {
            var pedidos = new List<Pedido_Produto>();
            try
            {
                using var conn = new SQLiteConnection(connectionString);
                conn.Open();

                string selectQuery = $@"SELECT PED.PedCod, EAN, PP.QuantidadePedida FROM Pedido PED INNER JOIN Pedido_Produto PP ON PP.PedCod = PED.PedCod WHERE PED.PEDCOD = {pedcod}";
                using var command = new SQLiteCommand(selectQuery, conn);

                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var pedido = new Pedido_Produto
                    {
                        PedCod = reader.GetInt32(0),
                        EAN = reader.GetString(1),
                        Quantidade = reader.GetInt32(2)
                    };

                    pedidos.Add(pedido);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return pedidos;
        }
        public static bool UpdatePedido(int pedcod, List<Pedido_Produto> produtos)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                foreach (Pedido_Produto produto in produtos)
                {
                    string insertPedidoProduto = $@"UPDATE Pedido_Produto SET QuantidadePedida = {produto.Quantidade} WHERE PedCod = {pedcod} AND EAN = '{produto.EAN}'";
                    using (var cmdPedido = new SQLiteCommand(insertPedidoProduto, conn, transaction))
                    {
                        cmdPedido.ExecuteNonQuery();
                    }
                }

                transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Console.WriteLine($"Erro: {ex.Message}");
                return false;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }
    }
}
