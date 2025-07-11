using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using SistemPlanilha.Repositorio;
using SistemPlanilha.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;

namespace SistemPlanilha.Controllers
{
    public class InventarioController : Controller
    {
        private readonly IInventarioRepositorio _inventarioRepositorio;
        private readonly BancoContext _bancoContext;

        public InventarioController(IInventarioRepositorio inventarioRepositorio, BancoContext bancoContext)
        {
            _inventarioRepositorio = inventarioRepositorio;
            _bancoContext = bancoContext;
        }

        private async Task PopularInventarioFormViewModelSelectLists(InventarioFormViewModel viewModel)
        {
            viewModel.Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", viewModel.SetorId);
            viewModel.Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", viewModel.TipoId);
            viewModel.Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", viewModel.SituacaoId);
            viewModel.WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", viewModel.WinVerId);
            viewModel.Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", viewModel.OfficeId);
        }

        public async Task<IActionResult> Index(string termo, string filtroSO = "todos", string filtroOffice = "todos",
                                             int? setorId = null, int? tipoId = null, int? situacaoId = null,
                                             int? winVerId = null, int? officeId = null,
                                             int page = 1, int pageSize = 10)
        {
            var viewModel = new InventarioIndexViewModel
            {
                Inventarios = new List<InventarioModel>(),
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = 0,
                TermoAtual = termo,
                FiltroSO = filtroSO,
                FiltroOffice = filtroOffice,
                SetorId = setorId,
                TipoId = tipoId,
                SituacaoId = situacaoId,
                WinVerId = winVerId,
                OfficeId = officeId,
                // CORREÇÃO: Adicionado AsNoTracking() para consistência e performance.
                Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", setorId),
                Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", tipoId),
                Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", situacaoId),
                WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", winVerId),
                Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", officeId)
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> BuscarInventario(string termo, string filtroSO, string filtroOffice,
                                                         int? setorId = null, int? tipoId = null, int? situacaoId = null,
                                                         int? winVerId = null, int? officeId = null,
                                                         int page = 1, int pageSize = 10)
        {
            var query = _inventarioRepositorio.Buscar(termo, filtroSO, filtroOffice, setorId, tipoId, situacaoId, winVerId, officeId);

            var totalItens = await query.CountAsync();
            var totalPages = (int)Math.Ceiling((double)totalItens / pageSize);

            if (page > totalPages && totalPages > 0) page = totalPages;

            var itensPaginados = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            // CORREÇÃO: Renderizar a paginação no servidor para simplificar o JavaScript.
            var viewModelParaPartials = new InventarioIndexViewModel
            {
                CurrentPage = page,
                PageSize = pageSize,
                TotalPages = totalPages,
                TermoAtual = termo,
                FiltroSO = filtroSO,
                FiltroOffice = filtroOffice,
                SetorId = setorId,
                TipoId = tipoId,
                SituacaoId = situacaoId,
                WinVerId = winVerId,
                OfficeId = officeId
            };

            var tabelaHtml = await RenderViewToStringAsync("_TabelaInventario", itensPaginados);

            return Json(new
            {
                tabelaHtml,
                currentPage = page,
                totalPages
            });
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            var inventario = _inventarioRepositorio.ListarPorId(id);
            if (inventario == null) return NotFound();
            var relatorios = _bancoContext.RelatoriosManutencao.Where(r => r.InventarioId == id).Include(r => r.StatusManutencao).ToList();
            var viewModel = new InventarioDetalhesViewModel { Inventario = inventario, Relatorios = relatorios };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Criar()
        {
            var viewModel = new InventarioFormViewModel();
            await PopularInventarioFormViewModelSelectLists(viewModel);
            return View("Criar", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(InventarioFormViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                await PopularInventarioFormViewModelSelectLists(viewModel);
                return View("Criar", viewModel);
            }
            var novoItem = new InventarioModel { /* ... mapeamento ... */ }; // Seu mapeamento aqui
            _inventarioRepositorio.Adicionar(novoItem);
            TempData["MensagemSucesso"] = "Item criado com sucesso!";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            var itemDoBanco = _inventarioRepositorio.ListarPorId(id);
            if (itemDoBanco == null)
            {
                return NotFound();
            }

            // CORREÇÃO: Mapeando os dados do item buscado para o ViewModel
            var viewModel = new InventarioFormViewModel
            {
                Id = itemDoBanco.Id,
                PcName = itemDoBanco.PcName,
                Serial = itemDoBanco.Serial,
                Patrimonio = itemDoBanco.Patrimonio,
                Usuario = itemDoBanco.Usuario,
                Modelo = itemDoBanco.Modelo,
                Responsavel = itemDoBanco.Responsavel,
                LicencaSO = itemDoBanco.LicencaSO,
                LicencaOffice = itemDoBanco.LicencaOffice,
                Processador = itemDoBanco.Processador,
                Ssd = itemDoBanco.Ssd,
                Obs = itemDoBanco.Obs,
                SetorId = itemDoBanco.SetorId,
                TipoId = itemDoBanco.TipoId,
                SituacaoId = itemDoBanco.SituacaoId,
                WinVerId = itemDoBanco.WinVerId,
                OfficeId = itemDoBanco.OfficeId,
            };

            // Este método popula os <select> (dropdowns) do formulário
            await PopularInventarioFormViewModelSelectLists(viewModel);

            // Envia o ViewModel PREENCHIDO para a View "Editar.cshtml"
            return View("Editar", viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(InventarioFormViewModel viewModel) // Alterado para async para consistência
        {
            try
            {
                // Primeiro, verifica se os dados do formulário são válidos (ex: campos obrigatórios)
                if (!ModelState.IsValid)
                {
                    // Se não for válido, repopula os dropdowns e retorna para a mesma tela de edição
                    await PopularInventarioFormViewModelSelectLists(viewModel);
                    return View("Editar", viewModel);
                }

                // CORREÇÃO: Mapeia os dados recebidos do formulário (viewModel) para um objeto do tipo InventarioModel
                var itemParaAtualizar = new InventarioModel
                {
                    Id = viewModel.Id, // O ID é crucial para saber qual item atualizar
                    PcName = viewModel.PcName,
                    Serial = viewModel.Serial,
                    Patrimonio = viewModel.Patrimonio,
                    Usuario = viewModel.Usuario,
                    Modelo = viewModel.Modelo,
                    Responsavel = viewModel.Responsavel,
                    LicencaSO = viewModel.LicencaSO,
                    LicencaOffice = viewModel.LicencaOffice,
                    Processador = viewModel.Processador,
                    Ssd = viewModel.Ssd,
                    Obs = viewModel.Obs,
                    SetorId = viewModel.SetorId,
                    TipoId = viewModel.TipoId,
                    SituacaoId = viewModel.SituacaoId,
                    WinVerId = viewModel.WinVerId,
                    OfficeId = viewModel.OfficeId,
                };

                // Envia o objeto COMPLETO E ATUALIZADO para o método do repositório
                _inventarioRepositorio.Atualizar(itemParaAtualizar);

                TempData["MensagemSucesso"] = "Item atualizado com sucesso!";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível atualizar o item. Detalhe: {erro.Message}";
                // Em caso de erro, é bom retornar à view de edição para não perder os dados digitados
                await PopularInventarioFormViewModelSelectLists(viewModel);
                return View("Editar", viewModel);
            }
        }

        [HttpGet]
        public IActionResult Apagar(int id)
        {
            var itemParaApagar = _inventarioRepositorio.ListarPorId(id);
            if (itemParaApagar == null) return NotFound();
            return View(itemParaApagar);
        }

        [HttpPost, ActionName("Apagar")]
        [ValidateAntiForgeryToken]
        public IActionResult ApagarConfirmado(int id)
        {
            try
            {
                _inventarioRepositorio.Apagar(id);
                TempData["MensagemSucesso"] = "Item apagado com sucesso!";
            }
            catch (Exception erro)
            {
                TempData["MensagemErro"] = $"Ops, não foi possível apagar o item. Detalhe: {erro.Message}";
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> RenderViewToStringAsync(string viewName, object model)
        {
           
            if (string.IsNullOrEmpty(viewName))
                viewName = ControllerContext.ActionDescriptor.ActionName;

            ViewData.Model = model;

            using (var writer = new StringWriter())
            {
                try
                {
                    var viewEngine = HttpContext.RequestServices.GetRequiredService<ICompositeViewEngine>();
                    var result = viewEngine.FindView(ControllerContext, viewName, false);
                    if (!result.Success)
                    {
                        var searched = string.Join("\n", result.SearchedLocations);
                        throw new InvalidOperationException($"View '{viewName}' não encontrada. Pesquisado em:\n{searched}");
                    }
                    var viewContext = new ViewContext(ControllerContext, result.View, ViewData, TempData, writer, new HtmlHelperOptions());
                    await result.View.RenderAsync(viewContext);
                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception ex)
                {
                    return $"";
                }
            }
        }
    }
}
