namespace API_Leitura_Arquivo.Model
{
    public class Pedido
    {
        public int PedCod { get; set; }
        public string CNPJ { get; set; }
    }
    public class Pedido_Produto
    {
        public int PedCod { get; set; }
        public string EAN { get; set; }
        public int Quantidade { get; set; }
    }
}
