using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Application;
using SistemPlanilha.ViewModels.Inventario;
using System;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IInventarioApp _inventarioAppService;

        
        public InventarioController(IInventarioApp inventarioAppService)
        {
            _inventarioAppService = inventarioAppService;
        }

        public async Task<IActionResult> Index(string termo, string filtroSO = "todos", string filtroOffice = "todos", int? setorId = null, int? tipoId = null, int? situacaoId = null, int? winVerId = null, int? officeId = null, int page = 1, int pageSize = 10)
        {
            var vm = await _inventarioAppService.ObterDadosPaginaIndex(termo, filtroSO, filtroOffice, setorId, tipoId, situacaoId, winVerId, officeId, page, pageSize);
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var vm = await _inventarioAppService.ObterDetalhesInventario(id);
            if (vm == null)
            {
                return NotFound();
            }
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var viewModelParaExibir = await _inventarioAppService.PrepararViewModelParaCriar();
            return View(viewModelParaExibir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar([FromForm] CriarInventarioCommand command)
        {
            if (!ModelState.IsValid)
            {
               
                var viewModelParaExibir = await _inventarioAppService.PrepararViewModelParaCriar();
                return View(viewModelParaExibir);
            }

            await _inventarioAppService.AdicionarInventario(command);
            TempData["MensagemSucesso"] = "Item criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var viewModelParaExibir = await _inventarioAppService.PrepararViewModelParaEditar(id);
            if (viewModelParaExibir == null)
            {
                return NotFound();
            }
            return PartialView("_EditarModalPartial", viewModelParaExibir);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, [FromForm] EditarInventarioCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                var viewModelParaExibir = await _inventarioAppService.PrepararViewModelParaEditar(id);
                
                viewModelParaExibir.Command = command;
                return PartialView("_EditarModalPartial", viewModelParaExibir);
            }

            try
            {
                await _inventarioAppService.AtualizarInventario(command);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Apagar(int id)
        {
            var item = await _inventarioAppService.ObterModelPorId(id);
            if (item == null)
            {
                return NotFound();
            }
            return PartialView("_ApagarModalPartial", item);
        }

        [HttpPost, ActionName("Apagar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApagarConfirmado(int id)
        {
            try
            {
                
                var usuarioLogado = User.Identity?.Name ?? "Usuário Desconhecido";

                await _inventarioAppService.ApagarInventario(id);
                return Json(new { success = true, message = "Item apagado!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}