using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using API_Leitura_Arquivo.Model;

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

        [HttpGet]
        public IActionResult Get()
        {
            return StatusCode(200, "tidp");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePedido(int id, [FromBody] Pedido pedido)
        {
            try
            {
                pedido.PedCod = id;
                _logger.LogInformation("Requisi��o PUT recebida para atualizar o pedido ID: {Id}", id);

                if (pedido == null || id != pedido.PedCod)
                {
                    _logger.LogWarning("Dados inv�lidos ou ID n�o corresponde ao corpo da requisi��o.");
                    return BadRequest("Dados inv�lidos ou ID n�o corresponde.");
                }


                _logger.LogInformation("Pedido ID {Id} atualizado com sucesso.", id);
                return NoContent(); // Retorna 204 No Content para indicar sucesso
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao processar a requisi��o PUT para o pedido ID: {Id}", id);
                return StatusCode(500, "Erro interno do servidor.");
            }
        }
    }
}
