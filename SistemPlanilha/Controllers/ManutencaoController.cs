using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Rotativa.AspNetCore;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Manutencao;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    
    public class ManutencaoController : Controller
    {
        private readonly IManutencaoApp _manutencaoApp;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ManutencaoController(IManutencaoApp manutencaoApp, IWebHostEnvironment webHostEnvironment)
        {
            _manutencaoApp = manutencaoApp;
            _webHostEnvironment = webHostEnvironment;
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

        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> Create(int? inventarioId)
        {
            var viewModel = await _manutencaoApp.PrepararViewModelParaCriar(inventarioId);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Tecnico")]
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

        [Authorize(Roles = "Administrador,Tecnico")]
        public async Task<IActionResult> Edit(int id)
        {
            var viewModel = await _manutencaoApp.PrepararViewModelParaEditar(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador,Tecnico")]
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

        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Delete(int id)
        {
            var viewModel = await _manutencaoApp.ObterDadosParaApagar(id);
            if (viewModel == null) return NotFound();
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> DeleteConfirmed(int id, int inventarioId)
        {
            await _manutencaoApp.DeletarManutencaoLogicamente(id);
            return RedirectToAction(nameof(Index), new { inventarioId = inventarioId });
        }

        public async Task<IActionResult> ExportarPdf(int? inventarioId)
        {
            var viewModel = await _manutencaoApp.ObterTodosParaListagem(inventarioId);
            string nomeArquivo = $"Manutencoes_{DateTime.Now:yyyy-MM-dd}.pdf";
            if (inventarioId.HasValue)
            {
                nomeArquivo = $"Manutencoes_Inventario_{inventarioId}_{DateTime.Now:yyyy-MM-dd}.pdf";
            }
            return new ViewAsPdf("_ManutencaoPdf", viewModel)
            {
                FileName = nomeArquivo,
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        public async Task<IActionResult> Pdf(int id)
        {
            var viewModel = await _manutencaoApp.ObterDetalhes(id);
            if (viewModel == null)
            {
                return NotFound();
            }

            string caminhoDaImagem = Path.Combine(_webHostEnvironment.WebRootPath, "images", "LogoCrea.png");
            ViewBag.CaminhoImagemLogo = caminhoDaImagem;

            return new ViewAsPdf("_ManutencaoDetalhesPdf", viewModel)
            {
                FileName = $"Manutencao_{id}.pdf",
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                CustomSwitches = "--enable-local-file-access"
            };
        }
    }
}