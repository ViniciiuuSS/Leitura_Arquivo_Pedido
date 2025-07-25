namespace Leitura_Arquivos_Pedido.Model
{
    public class Pedido
    {
        public int PedCod { get; set; }
        public string CNPJ { get; set; }
        public string DataPedido { get; set; }

        public Cliente Cliente { get; set; }
        public List<Pedido_Produto> Itens { get; set; }
    }
    public class Pedido_Produto
    {
        public Produto Produto { get; set; }
    }
}
