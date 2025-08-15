using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Data.Helpers;
using SistemPlanilha.Domain.Services;
using SistemPlanilha.Models;
using SistemPlanilha.Repositorio;
using SistemPlanilha.ViewModels.Inventario;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public class InventarioApp : IInventarioApp
    {
        private readonly IInventarioRepositorio _inventarioRepositorio;
        private readonly IInventarioService _inventarioService;
        private readonly IAuditService _auditService; // <-- Injetado
        private readonly BancoContext _bancoContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public InventarioApp(
            IInventarioRepositorio inventarioRepositorio,
            IInventarioService inventarioService,
            IAuditService auditService, // <-- Injetado
            BancoContext bancoContext,
            IHttpContextAccessor httpContextAccessor)
        {
            _inventarioRepositorio = inventarioRepositorio;
            _inventarioService = inventarioService;
            _auditService = auditService; // <-- Atribuído
            _bancoContext = bancoContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private string ObterUsuarioLogado()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "gabriel.felix";
        }

        public async Task AdicionarInventario(CriarInventarioCommand command)
        {
            await _inventarioService.ValidarPatrimonioParaCriacao(command.Patrimonio);

            var novoInventario = new InventarioModel
            {
                PcName = command.PcName,
                Serial = command.Serial,
                Patrimonio = command.Patrimonio,
                Usuario = command.Usuario,
                Modelo = command.Modelo,
                PrevisaoDevolucao = command.PrevisaoDevolucao,
                Responsavel = command.Responsavel,
                Processador = command.Processador,
                Ssd = command.Ssd,
                Obs = command.Obs,
                LicencaSO = command.LicencaSO,
                LicencaOffice = command.LicencaOffice,
                SetorId = command.SetorId,
                TipoId = command.TipoId,
                SituacaoId = command.SituacaoId,
                WinVerId = command.WinVerId,
                OfficeId = command.OfficeId,
                DataCriacao = DateTime.UtcNow,
                CriadoPor = ObterUsuarioLogado()
            };

            await _inventarioRepositorio.Adicionar(novoInventario);

            // REGISTRA A AUDITORIA
            await _auditService.RegistrarAuditoria(
                TipoAcao.Criacao, "Inventarios", novoInventario.Id, ObterUsuarioLogado(), command);
        }

        public async Task AtualizarInventario(EditarInventarioCommand command)
        {
            await _inventarioService.ValidarPatrimonioParaEdicao(command.Patrimonio, command.Id);

            var existente = await _inventarioRepositorio.ListarPorId(command.Id);
            if (existente == null) { throw new Exception("Item não encontrado."); }

            existente.PcName = command.PcName; existente.Serial = command.Serial; existente.Patrimonio = command.Patrimonio;
            existente.Usuario = command.Usuario; existente.Modelo = command.Modelo; existente.PrevisaoDevolucao = command.PrevisaoDevolucao;
            existente.Responsavel = command.Responsavel; existente.Processador = command.Processador; existente.Ssd = command.Ssd;
            existente.Obs = command.Obs; existente.LicencaSO = command.LicencaSO; existente.LicencaOffice = command.LicencaOffice;
            existente.SetorId = command.SetorId; existente.TipoId = command.TipoId; existente.SituacaoId = command.SituacaoId;
            existente.WinVerId = command.WinVerId; existente.OfficeId = command.OfficeId;
            existente.DataAtualizacao = DateTime.UtcNow;
            existente.AtualizadoPor = ObterUsuarioLogado();

            await _inventarioRepositorio.Atualizar(existente);

            // REGISTRA A AUDITORIA
            await _auditService.RegistrarAuditoria(
                TipoAcao.Atualizacao, "Inventarios", existente.Id, ObterUsuarioLogado(), command);
        }

        public async Task ApagarInventario(int id)
        {
            var itemParaApagar = await _inventarioRepositorio.ListarPorId(id);
            if (itemParaApagar != null)
            {
                var usuario = ObterUsuarioLogado();
                itemParaApagar.DeletadoEm = DateTime.UtcNow;
                itemParaApagar.DeletadoPor = usuario;
                await _inventarioRepositorio.Atualizar(itemParaApagar);

                // REGISTRA A AUDITORIA
                await _auditService.RegistrarAuditoria(
                    TipoAcao.DelecaoLogica, "Inventarios", id, usuario);
            }
        }

        public async Task<InventarioApiDto?> ObterInventarioDtoPorId(int id)
        {
            var inventario = await _inventarioRepositorio.ListarPorId(id);
            if (inventario == null) return null;
            return new InventarioApiDto
            {
                Id = inventario.Id,
                PcName = inventario.PcName,
                Usuario = inventario.Usuario,
                Patrimonio = inventario.Patrimonio,
                Serial = inventario.Serial,
                Modelo = inventario.Modelo,
                Processador = inventario.Processador,
                Ssd = inventario.Ssd,
                SetorNome = inventario.Setor?.Nome,
                TipoNome = inventario.Tipo?.Nome,
                SituacaoNome = inventario.Situacao?.Nome,
                WinVerNome = inventario.WinVer?.Nome,
                OfficeNome = inventario.Office?.Nome
            };
        }

        public async Task<InventarioModel?> ObterModelPorId(int id)
        {
            return await _inventarioRepositorio.ListarPorId(id);
        }

        public async Task<InventarioIndexViewModel> ObterDadosPaginaIndex(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId, int page, int pageSize)
        {
            var query = _inventarioRepositorio.Buscar(termo, filtroSO, filtroOffice, setorId, tipoId, situacaoId, winVerId, officeId).AsNoTracking();
            var pagedListOfModels = await PaginatedList<InventarioModel>.CreateAsync(query, page, pageSize);
            var listaDeViewModels = pagedListOfModels.Select(item => new InventarioParaListagemViewModel
            {
                Id = item.Id,
                PcName = item.PcName,
                Usuario = item.Usuario,
                Patrimonio = item.Patrimonio,
                Serial = item.Serial,
                Modelo = item.Modelo,
                SetorNome = item.Setor?.Nome,
                SituacaoNome = item.Situacao?.Nome,
                Responsavel = item.Responsavel,
                LicencaSO = item.LicencaSO,
                WindowsNome = item.WinVer?.Nome,
                OfficeNome = item.Office?.Nome,
                LicencaOffice = item.LicencaOffice,
                Processador = item.Processador,
                Ssd = item.Ssd,
                Obs = item.Obs,
                TipoNome = item.Tipo?.Nome
            }).ToList();
            var pagedListOfViewModels = new PaginatedList<InventarioParaListagemViewModel>(listaDeViewModels, pagedListOfModels.TotalCount, pagedListOfModels.PageIndex, pageSize);
            var viewModelFinal = new InventarioIndexViewModel
            {
                Inventarios = pagedListOfViewModels,
                TermoAtual = termo,
                FiltroSO = filtroSO,
                FiltroOffice = filtroOffice,
                SetorId = setorId,
                TipoId = tipoId,
                SituacaoId = situacaoId,
                WinVerId = winVerId,
                OfficeId = officeId,
                PageSize = pageSize,
                CurrentPage = pagedListOfModels.PageIndex,
                TotalPages = pagedListOfModels.TotalPages,
                Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", setorId),
                Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", tipoId),
                Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", situacaoId),
                WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", winVerId),
                Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", officeId)
            };
            return viewModelFinal;
        }

        public async Task<InventarioDetalhesViewModel?> ObterDetalhesInventario(int id)
        {
            var inventario = await _inventarioRepositorio.ListarPorId(id);
            if (inventario == null) return null;
            var relatorios = await _bancoContext.Manutencoes.Where(r => r.InventarioId == id).Include(r => r.StatusManutencao).ToListAsync();
            return new InventarioDetalhesViewModel { Inventario = inventario, Relatorios = relatorios };
        }

        public async Task<ExibirInventarioFormViewModel> PrepararViewModelParaCriar()
        {
            var vm = new ExibirInventarioFormViewModel();
            await PopularSelectListsParaCriacao(vm);
            return vm;
        }

        public async Task<ExibirInventarioEditarFormViewModel?> PrepararViewModelParaEditar(int id)
        {
            var item = await _inventarioRepositorio.ListarPorId(id);
            if (item == null) return null;
            var command = new EditarInventarioCommand
            {
                Id = item.Id,
                PcName = item.PcName,
                Serial = item.Serial,
                Patrimonio = item.Patrimonio,
                Usuario = item.Usuario,
                Modelo = item.Modelo,
                PrevisaoDevolucao = item.PrevisaoDevolucao,
                Responsavel = item.Responsavel,
                Processador = item.Processador,
                Ssd = item.Ssd,
                Obs = item.Obs,
                LicencaSO = item.LicencaSO,
                LicencaOffice = item.LicencaOffice,
                SetorId = item.SetorId,
                TipoId = item.TipoId,
                SituacaoId = item.SituacaoId,
                WinVerId = item.WinVerId,
                OfficeId = item.OfficeId
            };
            var viewModelParaExibir = new ExibirInventarioEditarFormViewModel { Command = command };
            await PopularSelectListsParaEdicao(viewModelParaExibir);
            return viewModelParaExibir;
        }

        private async Task PopularSelectListsParaCriacao(ExibirInventarioFormViewModel vm)
        {
            vm.Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome");
            vm.Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome");
            vm.Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome");
            vm.WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome");
            vm.Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome");
        }

        private async Task PopularSelectListsParaEdicao(ExibirInventarioEditarFormViewModel vm)
        {
            vm.Setores = new SelectList(await _bancoContext.Setores.AsNoTracking().ToListAsync(), "Id", "Nome", vm.Command.SetorId);
            vm.Tipos = new SelectList(await _bancoContext.Tipos.AsNoTracking().ToListAsync(), "Id", "Nome", vm.Command.TipoId);
            vm.Situacoes = new SelectList(await _bancoContext.Situacoes.AsNoTracking().ToListAsync(), "Id", "Nome", vm.Command.SituacaoId);
            vm.WinVers = new SelectList(await _bancoContext.WinVer.AsNoTracking().ToListAsync(), "Id", "Nome", vm.Command.WinVerId);
            vm.Offices = new SelectList(await _bancoContext.Office.AsNoTracking().ToListAsync(), "Id", "Nome", vm.Command.OfficeId);
        }
    }
}