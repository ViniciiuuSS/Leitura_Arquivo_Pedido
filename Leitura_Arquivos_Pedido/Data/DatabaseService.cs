using Leitura_Arquivos_Pedido.Model;
using System.Data.Common;
using System.Data.SQLite;
using System.Transactions;

namespace Leitura_Arquivos_Pedido.Data
{
    public static class DatabaseService
    {
        private static readonly string dbFile = "database.db";
        private static readonly string connectionString = $"Data Source={dbFile};Version=3;";

        public static void Inicializar()
        {
            if (!File.Exists(dbFile))
            {
                SQLiteConnection.CreateFile(dbFile);
            }

            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            string createTableQuery = @"
                CREATE TABLE IF NOT EXISTS Arquivos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nome TEXT NOT NULL,
                    Caminho TEXT NOT NULL,
                    DataCadastro TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Pedido (
                    PedCod INTEGER PRIMARY KEY AUTOINCREMENT,
                    CliCod INTEGER NOT NULL,
                    DataPedido TEXT NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Pedido_Produto (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PedCod INTEGER NOT NULL,
                    EAN TEXT NOT NULL,
                    QuantidadePedida INTEGER NOT NULL
                );
                CREATE TABLE IF NOT EXISTS Cliente (
                    CliCod INTEGER PRIMARY KEY AUTOINCREMENT,
                    CNPJ TEXT NOT NULL
                );
                ";

            using var cmd = new SQLiteCommand(createTableQuery, conn);
            cmd.ExecuteNonQuery();
        }

        public static void InserirArquivo(string nome, string caminho, DateTime dataCadastro)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();
            try
            {

                string insertQuery = "INSERT INTO Arquivos (Nome, Caminho, DataCadastro) VALUES (@nome, @caminho, @data)";

                using var cmd = new SQLiteCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@nome", nome);
                cmd.Parameters.AddWithValue("@caminho", caminho);
                cmd.Parameters.AddWithValue("@data", dataCadastro.ToString("yyyy-MM-dd HH:mm:ss"));

                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro: {ex.Message}");
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        public static bool InserirPedido(Pedido pedido)
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            using var transaction = conn.BeginTransaction();

            try
            {
                string insertCliente = "INSERT OR IGNORE INTO Cliente (CNPJ) VALUES (@cnpj)";
                using (var cmdCliente = new SQLiteCommand(insertCliente, conn, transaction))
                {
                    cmdCliente.Parameters.AddWithValue("@cnpj", pedido.Cliente.CNPJ);
                    cmdCliente.ExecuteNonQuery();
                }

                string verificaCliente = "SELECT CliCod FROM Cliente WHERE CNPJ = @cnpj";
                using var checkCmd = new SQLiteCommand(verificaCliente, conn, transaction);
                checkCmd.Parameters.AddWithValue("@cnpj", pedido.Cliente.CNPJ);

                long clicod = (long)checkCmd.ExecuteScalar();

                string insertPedido = @"INSERT INTO Pedido (CliCod, DataPedido) VALUES (@clicod, @data)";
                using (var cmdPedido = new SQLiteCommand(insertPedido, conn, transaction))
                {
                    cmdPedido.Parameters.AddWithValue("@clicod", clicod);
                    cmdPedido.Parameters.AddWithValue("@data", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                    cmdPedido.ExecuteNonQuery();
                }

                foreach(Pedido_Produto PedidoProduto in pedido.Itens)
                {
                    Produto produto = PedidoProduto.Produto;
                    string stringPedidoPedCod = "SELECT MAX(PedCod) FROM Pedido";
                    using var Cmd = new SQLiteCommand(stringPedidoPedCod, conn, transaction);

                    long pedCod = (long)Cmd.ExecuteScalar();

                    string insertPedidoProduto = @"INSERT INTO Pedido_Produto (PedCod, EAN, QuantidadePedida) VALUES (@pedcod, @ean, @quantidadePedida)";
                    using (var cmdPedido = new SQLiteCommand(insertPedidoProduto, conn, transaction))
                    {
                        cmdPedido.Parameters.AddWithValue("@pedcod", pedCod);
                        cmdPedido.Parameters.AddWithValue("@ean", produto.EAN);
                        cmdPedido.Parameters.AddWithValue("@quantidadePedida", produto.QTD);
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
        public static List<Pedido> ObterTodosPedidos()
        {
            using var conn = new SQLiteConnection(connectionString);
            conn.Open();

            var pedidos = new List<Pedido>();

            string selectQuery = @"SELECT PedCod, CNPJ, DataPedido FROM Pedido PED INNER JOIN Cliente CLI ON CLI.CliCod = PED.CliCod";
            using var command = new SQLiteCommand(selectQuery, conn);

            using var reader = command.ExecuteReader();
            while (reader.Read())
            {
                var pedido = new Pedido
                {
                    PedCod = reader.GetInt32(0),
                    CNPJ = reader.GetString(1),
                    DataPedido = reader.GetDateTime(2).ToString()
                };
                pedidos.Add(pedido);
            }

            return pedidos;
        }
    }
}
