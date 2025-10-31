using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Domain.Services;
using SistemPlanilha.Models;
using SistemPlanilha.ViewModels.Manutencao;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public class ManutencaoApp : IManutencaoApp
    {
        private readonly BancoContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IManutencaoService _manutencaoService;
        private readonly IAuditService _auditService;

        public ManutencaoApp(
            BancoContext context,
            IHttpContextAccessor httpContextAccessor,
            IManutencaoService manutencaoService,
            IAuditService auditService)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _manutencaoService = manutencaoService;
            _auditService = auditService;
        }

        private string ObterUsuarioLogado()
        {
            return _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "gabriel.felix";
        }

      

        public async Task CriarManutencao(CriarManutencaoCommand command)
        {
            await _manutencaoService.ValidarInventarioParaNovaManutencao(command.InventarioId);

            var manutencao = new ManutencaoModel
            {
                InventarioId = command.InventarioId,
                Descricao = command.Descricao,
                StatusManutencaoId = command.StatusManutencaoId,
                Responsavel = command.Responsavel,
                ResponsavelTecnico = command.ResponsavelTecnico,
                TipoManutencao = command.TipoManutencao,
                AcoesRealizadas = command.AcoesRealizadas ?? string.Empty,
                TempoEstimadoResolucao = command.TempoEstimadoResolucao ?? string.Empty,
                PecasSubstituidas = command.PecasSubstituidas ?? string.Empty,
                ObservacoesAdicionais = command.ObservacoesAdicionais ?? string.Empty,
                ProximaManutencao = command.ProximaManutencao,
                DataCriacao = DateTime.UtcNow,
                CriadoPor = ObterUsuarioLogado()
            };

            _context.Add(manutencao);
            await _context.SaveChangesAsync();

            
            await _auditService.RegistrarAuditoria(
                TipoAcao.Criacao,
                "Manutencoes",
                manutencao.Id,
                ObterUsuarioLogado(),
                new
                {
                    mensagem = "Registro criado",
                    novo = Snapshot(manutencao)
                }
            );
        }

        public async Task AtualizarManutencao(EditarManutencaoCommand command)
        {
            var manutencao = await _context.Manutencoes.FindAsync(command.Id);
            if (manutencao != null)
            {
                
                var antes = Snapshot(manutencao);

               
                manutencao.InventarioId = command.InventarioId;
                manutencao.Descricao = command.Descricao;
                manutencao.StatusManutencaoId = command.StatusManutencaoId;
                manutencao.Responsavel = command.Responsavel;
                manutencao.ResponsavelTecnico = command.ResponsavelTecnico;
                manutencao.TipoManutencao = command.TipoManutencao;
                manutencao.AcoesRealizadas = command.AcoesRealizadas ?? string.Empty;
                manutencao.TempoEstimadoResolucao = command.TempoEstimadoResolucao ?? string.Empty;
                manutencao.PecasSubstituidas = command.PecasSubstituidas ?? string.Empty;
                manutencao.ObservacoesAdicionais = command.ObservacoesAdicionais ?? string.Empty;
                manutencao.ProximaManutencao = command.ProximaManutencao;

                manutencao.DataAtualizacao = DateTime.UtcNow;
                manutencao.AtualizadoPor = ObterUsuarioLogado();

                _context.Update(manutencao);
                await _context.SaveChangesAsync();

                
                var depois = Snapshot(manutencao);

                
                await _auditService.RegistrarAuditoria(
                    TipoAcao.Atualizacao,
                    "Manutencoes",
                    manutencao.Id,
                    ObterUsuarioLogado(),
                    new
                    {
                        mensagem = "Registro atualizado",
                        antes,
                        depois
                    }
                );
            }
        }

        public async Task DeletarManutencaoLogicamente(int id)
        {
            var manutencao = await _context.Manutencoes
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(r => r.Id == id);

            if (manutencao != null)
            {
                var usuario = ObterUsuarioLogado();
                manutencao.DeletadoEm = DateTime.UtcNow;
                manutencao.DeletadoPor = usuario;

                _context.Update(manutencao);
                await _context.SaveChangesAsync();

                
                await _auditService.RegistrarAuditoria(
                    TipoAcao.DelecaoLogica,
                    "Manutencoes",
                    id,
                    usuario,
                    new
                    {
                        mensagem = "Registro deletado logicamente",
                        deletadoEm = manutencao.DeletadoEm
                    }
                );
            }
        }

        

        #region Métodos de Busca e Preparação

        public async Task<IEnumerable<RelatorioParaListagemViewModel>> ObterTodosParaListagem(int? inventarioId)
        {
            var query = _context.Manutencoes
                .Include(r => r.Inventario)
                .Include(r => r.StatusManutencao)
                .AsQueryable();

            if (inventarioId.HasValue)
            {
                query = query.Where(r => r.InventarioId == inventarioId.Value);
            }

            return await query.Select(r => new RelatorioParaListagemViewModel
            {
                Id = r.Id,
                InventarioId = r.InventarioId,
                PcNameInventario = r.Inventario.PcName,
                Descricao = r.Descricao,
                DataCriacao = r.DataCriacao.ToString("dd/MM/yyyy"),
                ResponsavelTecnico = r.ResponsavelTecnico,
                StatusNome = r.StatusManutencao.Nome,
                Responsavel = r.Responsavel,
                TipoManutencao = r.TipoManutencao,
                AcoesRealizadas = r.AcoesRealizadas,
                TempoEstimadoResolucao = r.TempoEstimadoResolucao,
                ObservacoesAdicionais = r.ObservacoesAdicionais,
                ProximaManutencao = r.ProximaManutencao.HasValue ? r.ProximaManutencao.Value.ToString("dd/MM/yyyy") : "",
                PecasSubstituidas = r.PecasSubstituidas
            }).ToListAsync();
        }

        public async Task<RelatorioDetalhesViewModel?> ObterDetalhes(int id)
        {
            var manutencao = await _context.Manutencoes
                .Include(r => r.Inventario)
                .Include(r => r.StatusManutencao)
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.Id == id);

            if (manutencao == null) return null;

            return new RelatorioDetalhesViewModel
            {
                Id = manutencao.Id,
                InventarioId = manutencao.InventarioId,
                InventarioPcName = manutencao.Inventario?.PcName,
                Descricao = manutencao.Descricao,
                DataCriacao = manutencao.DataCriacao.ToString("dd/MM/yyyy"),
                Responsavel = manutencao.Responsavel,
                TipoManutencao = manutencao.TipoManutencao,
                AcoesRealizadas = manutencao.AcoesRealizadas,
                TempoEstimadoResolucao = manutencao.TempoEstimadoResolucao,
                StatusNome = manutencao.StatusManutencao?.Nome,
                ObservacoesAdicionais = manutencao.ObservacoesAdicionais,
                ProximaManutencao = manutencao.ProximaManutencao?.ToString("dd/MM/yyyy"),
                ResponsavelTecnico = manutencao.ResponsavelTecnico,
                PecasSubstituidas = manutencao.PecasSubstituidas
            };
        }

        public async Task<ExibirManutencaoFormViewModel> PrepararViewModelParaCriar(int? inventarioId)
        {
            var vm = new ExibirManutencaoFormViewModel();

            if (inventarioId.HasValue)
            {
                var inventario = await _context.Inventario.FindAsync(inventarioId.Value);
                if (inventario != null)
                {
                    vm.Command.InventarioId = inventario.Id;
                    vm.Command.Responsavel = inventario.Usuario;
                }
            }

            vm.InventarioItens = new SelectList(
                await _context.Inventario.OrderBy(i => i.PcName).ToListAsync(),
                "Id",
                "PcName",
                vm.Command.InventarioId
            );

            vm.StatusesManutencao = new SelectList(
                await _context.StatusesManutencao.ToListAsync(),
                "Id",
                "Nome",
                vm.Command.StatusManutencaoId
            );

            return vm;
        }

        public async Task<ExibirManutencaoFormViewModel?> PrepararViewModelParaEditar(int id)
        {
            var manutencao = await _context.Manutencoes.FindAsync(id);
            if (manutencao == null) return null;

            var command = new EditarManutencaoCommand
            {
                Id = manutencao.Id,
                InventarioId = manutencao.InventarioId,
                Descricao = manutencao.Descricao,
                StatusManutencaoId = manutencao.StatusManutencaoId,
                Responsavel = manutencao.Responsavel,
                ResponsavelTecnico = manutencao.ResponsavelTecnico,
                AcoesRealizadas = manutencao.AcoesRealizadas,
                TipoManutencao = manutencao.TipoManutencao,
                TempoEstimadoResolucao = manutencao.TempoEstimadoResolucao,
                PecasSubstituidas = manutencao.PecasSubstituidas,
                ObservacoesAdicionais = manutencao.ObservacoesAdicionais,
                ProximaManutencao = manutencao.ProximaManutencao
            };

            var vm = new ExibirManutencaoFormViewModel { Command = command };

            vm.InventarioItens = new SelectList(
                await _context.Inventario.OrderBy(i => i.PcName).ToListAsync(),
                "Id",
                "PcName",
                vm.Command.InventarioId
            );

            vm.StatusesManutencao = new SelectList(
                await _context.StatusesManutencao.ToListAsync(),
                "Id",
                "Nome",
                vm.Command.StatusManutencaoId
            );

            return vm;
        }

        public async Task<RelatorioParaApagarViewModel?> ObterDadosParaApagar(int id)
        {
            return await _context.Manutencoes
                .Where(r => r.Id == id)
                .Select(r => new RelatorioParaApagarViewModel
                {
                    Id = r.Id,
                    InventarioId = r.InventarioId,
                    Descricao = r.Descricao,
                    ResponsavelTecnico = r.ResponsavelTecnico
                })
                .FirstOrDefaultAsync();
        }

        #endregion

        
        private object Snapshot(ManutencaoModel m) => new
        {
            m.Id,
            m.InventarioId,
            m.Descricao,
            m.StatusManutencaoId,
            m.Responsavel,
            m.ResponsavelTecnico,
            m.TipoManutencao,
            m.AcoesRealizadas,
            m.TempoEstimadoResolucao,
            m.PecasSubstituidas,
            m.ObservacoesAdicionais,
            m.ProximaManutencao
        };
    }
}
