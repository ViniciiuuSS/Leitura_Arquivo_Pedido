using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_Leitura_Arquivo.Model;
using API_Leitura_Arquivo.Data;

namespace API_Leitura_Arquivo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PedidoController : ControllerBase
    {
        private readonly ILogger<PedidoController> _logger;

        public PedidoController(ILogger<PedidoController> logger)
        {
            _logger = logger;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            List<Pedido_Produto> pedido = DatabaseService.ObterTodosPedidos(id);
            return StatusCode(200, pedido);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] List<Pedido_Produto> pedido)
        {
            try
            {
                if (pedido == null)
                {
                    _logger.LogWarning("Dados inválidos ou ID não corresponde ao corpo da requisição.");
                    return BadRequest("Dados inválidos ou ID não corresponde.");
                }

                DatabaseService.UpdatePedido(id, pedido);

                _logger.LogInformation("Pedido ID {Id} atualizado com sucesso.", id);
                return NoContent(); // Retorna 204 No Content para indicar sucesso
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a requisição PUT para o pedido ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
