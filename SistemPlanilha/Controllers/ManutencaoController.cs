using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Manutencao;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    public class ManutencaoController : Controller
    {
        private readonly IManutencaoApp _manutencaoApp;

        public ManutencaoController(IManutencaoApp manutencaoApp)
        {
            _manutencaoApp = manutencaoApp;
        }

        public async Task<IActionResult> Index(int? inventarioId)
        {
            var viewModel = await _manutencaoApp.ObterTodosParaListagem(inventarioId);
            return View(viewModel);
        }

        public async Task<IActionResult> Details(int id)
        {
            var viewModel = await _manutencaoApp.ObterDetalhes(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        public async Task<IActionResult> Create(int? inventarioId)
        {
            var viewModel = await _manutencaoApp.PrepararViewModelParaCriar(inventarioId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CriarManutencaoCommand command)
        {
            if (!ModelState.IsValid)
            {
                var viewModel = await _manutencaoApp.PrepararViewModelParaCriar(command.InventarioId);
                return View(viewModel);
            }
            await _manutencaoApp.CriarManutencao(command);
            return RedirectToAction(nameof(Index), new { inventarioId = command.InventarioId });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _manutencaoApp.PrepararViewModelParaEditar(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarManutencaoCommand command)
        {
            if (id != command.Id) return BadRequest();
            if (!ModelState.IsValid)
            {
                var viewModel = await _manutencaoApp.PrepararViewModelParaEditar(id);
                if (viewModel != null) { viewModel.Command = command; }
                return View(viewModel);
            }
            await _manutencaoApp.AtualizarManutencao(command);
            return RedirectToAction(nameof(Index), new { inventarioId = command.InventarioId });
        }

        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _manutencaoApp.ObterDadosParaApagar(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int inventarioId)
        {
            await _manutencaoApp.DeletarManutencaoLogicamente(id);
            return RedirectToAction(nameof(Index), new { inventarioId = inventarioId });
        }
    }
}