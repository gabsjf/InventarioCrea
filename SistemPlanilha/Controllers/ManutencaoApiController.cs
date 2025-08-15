using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Manutencao;
using System.Threading.Tasks;

[ApiController]
[Route("api/Manutencao")] // Rota já estava correta
public class ManutencaoApiController : ControllerBase
{
    private readonly IManutencaoApp _manutencaoApp;

    public ManutencaoApiController(IManutencaoApp manutencaoApp)
    {
        _manutencaoApp = manutencaoApp;
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetManutencao(int id) // Nome do método opcionalmente renomeado
    {
        var manutencao = await _manutencaoApp.ObterDetalhes(id);
        if (manutencao == null) return NotFound();
        return Ok(manutencao);
    }

    [HttpPost]
    public async Task<IActionResult> CreateManutencao([FromBody] CriarManutencaoCommand command)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // MUDANÇA AQUI
        await _manutencaoApp.CriarManutencao(command);
        return CreatedAtAction(nameof(GetManutencao), new { id = 0 }, command);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateManutencao(int id, [FromBody] EditarManutencaoCommand command)
    {
        if (id != command.Id) return BadRequest("ID da URL e do corpo não correspondem.");
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // MUDANÇA AQUI
        await _manutencaoApp.AtualizarManutencao(command);
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteManutencao(int id)
    {
        var item = await _manutencaoApp.ObterDadosParaApagar(id);
        if (item == null) return NotFound();

        // MUDANÇA AQUI
        await _manutencaoApp.DeletarManutencaoLogicamente(id);
        return NoContent();
    }
}