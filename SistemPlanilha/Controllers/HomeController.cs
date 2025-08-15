using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using SistemPlanilha.ViewModels;
using SistemPlanilha.ViewModels.Manutencao;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly BancoContext _context;

        public HomeController(BancoContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var totalItens = await _context.Inventario.CountAsync();
            var manutencoesAbertas = await _context.Manutencoes.CountAsync(m => m.StatusManutencao != null && m.StatusManutencao.Nome != "Concluído");
            var itensEmEstoque = await _context.Inventario.CountAsync(i => i.Situacao != null && i.Situacao.Nome == "Estoque");

            var dadosGrafico = await _context.Inventario
                .Where(i => i.Situacao != null && i.Situacao.Nome != null)
                .GroupBy(i => i.Situacao.Nome)
                .Select(g => new {
                    Situacao = g.Key,
                    Contagem = g.Count()
                })
                .OrderByDescending(x => x.Contagem)
                .ToListAsync();

            var ultimasManutencoes = await _context.Manutencoes
                .Include(m => m.Inventario)
                .Include(m => m.StatusManutencao)
                .OrderByDescending(m => m.DataCriacao)
                .Take(5)
                .Select(m => new RelatorioParaListagemViewModel
                {
                    Id = m.Id,
                    InventarioId = m.InventarioId,
                    PcNameInventario = m.Inventario.PcName,
                    Descricao = m.Descricao,
                    StatusNome = m.StatusManutencao.Nome,
                    DataCriacao = m.DataCriacao.ToString("dd/MM/yyyy HH:mm")
                })
                .ToListAsync();

            // --- DICIONÁRIO DE CORES CORRIGIDO ---
            var coresPorSituacao = new Dictionary<string, string>
            {
                { "Em uso", "rgba(54, 162, 235, 0.7)" },      // Azul
                { "Disponível", "rgba(75, 192, 192, 0.7)" },    // Verde-água
                { "Devolvido", "rgba(153, 102, 255, 0.7)" },  // Roxo
                { "Emprestado", "rgba(255, 159, 64, 0.7)" },     // Laranja
                { "Avariado", "rgba(255, 206, 86, 0.7)" }, // Amarelo
                { "Inativo", "rgba(255, 99, 132, 0.7)" },     // Vermelho
                { "Leilão", "rgba(201, 203, 207, 0.7)" }     // Cinza
            };

            var labelsGrafico = dadosGrafico.Select(d => d.Situacao).ToList();
            var dataGrafico = dadosGrafico.Select(d => d.Contagem).ToList();
            var coresGrafico = labelsGrafico.Select(label =>
                coresPorSituacao.ContainsKey(label) ? coresPorSituacao[label] : "rgba(108, 117, 125, 0.7)"
            ).ToList();

            var viewModel = new DashboardViewModel
            {
                TotalItensInventario = totalItens,
                ManutencoesAbertas = manutencoesAbertas,
                ItensEmEstoque = itensEmEstoque,
                UltimasManutencoes = ultimasManutencoes,
                GraficoSituacaoLabels = labelsGrafico,
                GraficoSituacaoData = dataGrafico,
                GraficoSituacaoColors = coresGrafico
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}