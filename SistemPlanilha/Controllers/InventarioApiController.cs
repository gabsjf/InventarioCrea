using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Inventario;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class InventarioApiController : ControllerBase
{
    private readonly IInventarioApp _inventarioAppService;

    public InventarioApiController(IInventarioApp inventarioAppService)
    {
        _inventarioAppService = inventarioAppService;
    }

    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetInventario(int id)
    {
       
        var inventarioDto = await _inventarioAppService.ObterInventarioDtoPorId(id);

        if (inventarioDto == null)
        {
            return NotFound();
        }

        return Ok(inventarioDto);
    }

    
    [HttpPost]
    public async Task<IActionResult> CreateInventario([FromBody] CriarInventarioCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _inventarioAppService.AdicionarInventario(command);

        
        return CreatedAtAction(nameof(GetInventario), new { id = 0 }, command);
    }

   
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateInventario(int id, [FromBody] EditarInventarioCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("O ID da URL não corresponde ao ID do corpo da requisição.");
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        await _inventarioAppService.AtualizarInventario(command);

        return NoContent();
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteInventario(int id)
    {
        var item = await _inventarioAppService.ObterInventarioDtoPorId(id);
        if (item == null)
        {
            return NotFound();
        }

        
        var usuarioLogado = User.Identity?.Name ?? "API_User";


        await _inventarioAppService.ApagarInventario(id);

        return NoContent();
    }
}