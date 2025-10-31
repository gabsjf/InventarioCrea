using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Inventario;
using System;
using System.Threading.Tasks;
using Rotativa.AspNetCore;

namespace SistemPlanilha.Controllers
{

    public class InventarioController : Controller
    {
        private readonly IInventarioApp _inventarioApp;

        public InventarioController(IInventarioApp inventarioApp)
        {
            _inventarioApp = inventarioApp;
        }

        public async Task<IActionResult> Index(string termo, string filtroSO = "todos", string filtroOffice = "todos", int? setorId = null, int? tipoId = null, int? situacaoId = null, int? winVerId = null, int? officeId = null, int page = 1, int pageSize = 10)
        {
            var vm = await _inventarioApp.ObterDadosPaginaIndex(termo, filtroSO, filtroOffice, setorId, tipoId, situacaoId, winVerId, officeId, page, pageSize);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _inventarioApp.ObterDetalhesInventario(id);
            if (vm == null) { return NotFound(); }
            return View(vm);
        }

        [Authorize(Roles = "Administrador,Tecnico")] 
        public async Task<IActionResult> ExportarPdf()
        {
            var viewModel = await _inventarioApp.ObterDadosPaginaIndex(
                termo: null, filtroSO: "todos", filtroOffice: "todos",
                setorId: null, tipoId: null, situacaoId: null, winVerId: null, officeId: null,
                page: 1, pageSize: int.MaxValue);

            return new ViewAsPdf("_InventarioPdf", viewModel)
            {
                FileName = $"Inventario_{DateTime.Now:yyyy-MM-dd}.pdf",
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4
            };
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> Criar()
        {
            var viewModelParaExibir = await _inventarioApp.PrepararViewModelParaCriar();
            return View(viewModelParaExibir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")] 
        public async Task<IActionResult> Criar([FromForm] CriarInventarioCommand command)
        {
            if (!ModelState.IsValid)
            {
                var viewModelParaExibir = await _inventarioApp.PrepararViewModelParaCriar();
                return View(viewModelParaExibir);
            }
            await _inventarioApp.AdicionarInventario(command);
            TempData["MensagemSucesso"] = "Item criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int id)
        {
            var viewModelParaExibir = await _inventarioApp.PrepararViewModelParaEditar(id);
            if (viewModelParaExibir == null) { return NotFound(); }
            return PartialView("_EditarModalPartial", viewModelParaExibir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Editar(int id, [FromForm] EditarInventarioCommand command)
        {
            if (id != command.Id) { return BadRequest(); }
            if (!ModelState.IsValid)
            {
                var viewModelParaExibir = await _inventarioApp.PrepararViewModelParaEditar(id);
                if (viewModelParaExibir != null)
                {
                    viewModelParaExibir.Command = command;
                }
                return PartialView("_EditarModalPartial", viewModelParaExibir);
            }
            try
            {
                await _inventarioApp.AtualizarInventario(command);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> Apagar(int id)
        {
            var item = await _inventarioApp.ObterModelPorId(id);
            if (item == null) { return NotFound(); }
            return PartialView("_ApagarModalPartial", item);
        }


        

        [HttpPost, ActionName("Apagar")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Administrador")]
        public async Task<IActionResult> ApagarConfirmado(int id)
        {
            try
            {
                await _inventarioApp.ApagarInventario(id);
                return Json(new { success = true, message = "Item apagado!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}