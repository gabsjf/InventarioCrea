using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Data.Helpers;      // <-- PaginatedList aqui
using SistemPlanilha.Models;
using SistemPlanilha.Repositorio;
using SistemPlanilha.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IInventarioRepositorio _inventarioRepositorio;
        private readonly BancoContext _bancoContext;

        public InventarioController(
            IInventarioRepositorio inventarioRepositorio,
            BancoContext bancoContext)
        {
            _inventarioRepositorio = inventarioRepositorio;
            _bancoContext = bancoContext;
        }

        private async Task PopularInventarioFormViewModelSelectLists(InventarioFormViewModel vm)
        {
            vm.Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", vm.SetorId);
            vm.Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", vm.TipoId);
            vm.Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", vm.SituacaoId);
            vm.WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", vm.WinVerId);
            vm.Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", vm.OfficeId);
        }

        // ---------- INDEX com PaginatedList (reload de página) ----------
        public async Task<IActionResult> Index(
            string termo,
            string filtroSO = "todos",
            string filtroOffice = "todos",
            int? setorId = null,
            int? tipoId = null,
            int? situacaoId = null,
            int? winVerId = null,
            int? officeId = null,
            int page = 1,
            int pageSize = 10)
        {
            // 1) prepara ViewModel com filtros e selects
            var vm = new InventarioIndexViewModel
            {
                TermoAtual = termo,
                FiltroSO = filtroSO,
                FiltroOffice = filtroOffice,
                SetorId = setorId,
                TipoId = tipoId,
                SituacaoId = situacaoId,
                WinVerId = winVerId,
                OfficeId = officeId,
                PageSize = pageSize,
                Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", setorId),
                Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", tipoId),
                Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", situacaoId),
                WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", winVerId),
                Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", officeId)
            };

            // 2) monta query com filtros
            var query = _inventarioRepositorio.Buscar(
                termo, filtroSO, filtroOffice,
                setorId, tipoId, situacaoId, winVerId, officeId
            ).AsNoTracking();

            // 3) cria PaginatedList
            var paged = await PaginatedList<InventarioModel>
                .CreateAsync(query, page, pageSize);

            // 4) preenche VM com itens e paginação
            vm.Inventarios = paged;
            vm.CurrentPage = paged.PageIndex;
            vm.TotalPages = paged.TotalPages;

            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var inventario = _inventarioRepositorio.ListarPorId(id);
            if (inventario == null) return NotFound();

            var rels = await _bancoContext.RelatoriosManutencao
                .Where(r => r.InventarioId == id)
                .Include(r => r.StatusManutencao)
                .ToListAsync();

            var vm = new InventarioDetalhesViewModel
            {
                Inventario = inventario,
                Relatorios = rels
            };
            return View(vm);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var vm = new InventarioFormViewModel();
            await PopularInventarioFormViewModelSelectLists(vm);
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(InventarioFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await PopularInventarioFormViewModelSelectLists(vm);
                return View(vm);
            }

            var novo = new InventarioModel
            {
                PcName = vm.PcName,
                Serial = vm.Serial,
                Patrimonio = vm.Patrimonio,
                Usuario = vm.Usuario,
                TipoId = vm.TipoId,
                SetorId = vm.SetorId,
                SituacaoId = vm.SituacaoId,
                WinVerId = vm.WinVerId,
                OfficeId = vm.OfficeId,
                LicencaSO = vm.LicencaSO,
                LicencaOffice = vm.LicencaOffice,
                Modelo = vm.Modelo,
                Responsavel = vm.Responsavel,
                Processador = vm.Processador,
                Ssd = vm.Ssd,
                Obs = vm.Obs
            };
            _inventarioRepositorio.Adicionar(novo);
            TempData["MensagemSucesso"] = "Item criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var item = _inventarioRepositorio.ListarPorId(id);
            if (item == null) return NotFound();

            var vm = new InventarioFormViewModel
            {
                Id = item.Id,
                PcName = item.PcName,
                Serial = item.Serial,
                Patrimonio = item.Patrimonio,
                Usuario = item.Usuario,
                TipoId = item.TipoId,
                SetorId = item.SetorId,
                SituacaoId = item.SituacaoId,
                WinVerId = item.WinVerId,
                OfficeId = item.OfficeId,
                LicencaSO = item.LicencaSO,
                LicencaOffice = item.LicencaOffice,
                Modelo = item.Modelo,
                Responsavel = item.Responsavel,
                Processador = item.Processador,
                Ssd = item.Ssd,
                Obs = item.Obs
            };
            await PopularInventarioFormViewModelSelectLists(vm);
            return PartialView("_EditarModalPartial", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(int id, InventarioFormViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await PopularInventarioFormViewModelSelectLists(vm);
                return PartialView("_EditarModalPartial", vm);
            }

            var existente = _inventarioRepositorio.ListarPorId(id);
            if (existente == null)
                return Json(new { success = false, message = "Item não encontrado." });

            existente.PcName = vm.PcName;
            existente.Serial = vm.Serial;
            existente.Patrimonio = vm.Patrimonio;
            existente.Usuario = vm.Usuario;
            existente.TipoId = vm.TipoId;
            existente.SetorId = vm.SetorId;
            existente.SituacaoId = vm.SituacaoId;
            existente.WinVerId = vm.WinVerId;
            existente.OfficeId = vm.OfficeId;
            existente.LicencaSO = vm.LicencaSO;
            existente.LicencaOffice = vm.LicencaOffice;
            existente.Modelo = vm.Modelo;
            existente.Responsavel = vm.Responsavel;
            existente.Processador = vm.Processador;
            existente.Ssd = vm.Ssd;
            existente.Obs = vm.Obs;

            _inventarioRepositorio.Atualizar(existente);
            return Json(new { success = true });
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            var item = _inventarioRepositorio.ListarPorId(id);
            if (item == null) return NotFound();
            return PartialView("_ApagarModalPartial", item);
        }

        [HttpPost, ActionName("Apagar")]
        [ValidateAntiForgeryToken]
        public IActionResult ApagarConfirmado(int id)
        {
            try
            {
                _inventarioRepositorio.Apagar(id);
                return Json(new { success = true, message = "Item apagado!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }
    }
}
