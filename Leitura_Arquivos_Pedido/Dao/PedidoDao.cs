using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leitura_Arquivos_Pedido.Dao
{
    public class PedidoDao
    {
        /// <summary>
        /// Formata uma string de CNPJ no padrão 00.000.000/0000-00.
        /// </summary>
        /// <param name="cnpj">CNPJ com ou sem pontuação (apenas números).</param>
        /// <returns>CNPJ formatado ou string original se inválido.</returns>
        public string FormatarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj))
                return cnpj;

            cnpj = new string(cnpj.Where(char.IsDigit).ToArray());

            if (cnpj.Length != 14)
                return cnpj;

            return Convert.ToUInt64(cnpj).ToString(@"00\.000\.000\/0000\-00");
        }
    }
}
