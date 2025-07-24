using Leitura_Arquivos_Pedido.Model;

namespace Leitura_Arquivos_Pedido.Util
{
    class Funcitons
    {
        public static Pedido ProcessarArquivo(string filePath)
        {
            Pedido pedido = new Pedido()
            {
                Cliente = new Cliente(),
                Itens = new List<Pedido_Produto>()
            };

            try
            {
                foreach (string linha in File.ReadAllLines(filePath))
                {
                    if (string.IsNullOrEmpty(linha)) { continue; }
                    string[] colunas = linha.Split(new string[] { ";" }, StringSplitOptions.None);
                    int lineType = int.Parse(colunas[0]);
                    #region HEADER
                    if (lineType == 1)
                    {
                        pedido.Cliente.CNPJ = colunas[1];
                    }
                    #endregion
                    #region ITENS
                    else if (lineType == 2)
                    {
                        Pedido_Produto item = new Pedido_Produto()
                        {
                            Produto = new Produto()
                        };
                        item.Produto.EAN = colunas[1];
                        item.Produto.QTD = int.Parse(colunas[2]);
                        pedido.Itens.Add(item);
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                pedido = null;
                MessageBox.Show($"Aquivo fora do padrão!", "Processamento Finalizado", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            return pedido;
        }
    }
}
