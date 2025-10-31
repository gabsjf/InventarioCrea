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
using System.Globalization;
using System.Text;
using System.Security.Cryptography;

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

            var manutencoesAbertas = await _context.Manutencoes
                .CountAsync(m => m.StatusManutencao != null && m.StatusManutencao.Nome != "Concluído");

            var itensEmEstoque = await _context.Inventario
                .CountAsync(i => i.Situacao != null && i.Situacao.Nome == "Estoque");

            var dadosGrafico = await _context.Inventario
                .Where(i => i.Situacao != null && i.Situacao.Nome != null)
                .GroupBy(i => i.Situacao.Nome)
                .Select(g => new
                {
                    Situacao = g.Key,
                    Contagem = g.Count()
                })
                .OrderByDescending(x => x.Contagem)
                .ToListAsync();

            var labelsGrafico = dadosGrafico.Select(d => d.Situacao).ToList();
            var dataGrafico = dadosGrafico.Select(d => d.Contagem).ToList();

           
            var coresPorSituacao = new Dictionary<string, string>
            {
                { NormalizeKey("Em uso"),     "rgba(54, 162, 235, 0.7)" },  // Azul
                { NormalizeKey("Disponível"), "rgba(75, 192, 192, 0.7)" },  // Verde-água
                { NormalizeKey("Devolvido"),  "rgba(153, 102, 255, 0.7)" }, // Roxo
                { NormalizeKey("Emprestado"), "rgba(255, 159, 64, 0.7)" },  // Laranja
                { NormalizeKey("Avariado"),   "rgba(255, 206, 86, 0.7)" },  // Amarelo
                { NormalizeKey("Inativo"),    "rgba(255, 99, 132, 0.7)" },  // Vermelho
                { NormalizeKey("Leilão"),     "rgba(201, 203, 207, 0.7)" }, // Cinza
                { NormalizeKey("Estoque"),    "rgba(0, 201, 167, 0.7)" }    // Verde água escuro (novo)
            };

           
            var coresGrafico = labelsGrafico.Select(label =>
            {
                var key = NormalizeKey(label);
                if (coresPorSituacao.TryGetValue(key, out var cor))
                    return cor;

                return HslFromString(key); 
            }).ToList();

            var viewModel = new DashboardViewModel
            {
                TotalItensInventario = totalItens,
                ManutencoesAbertas = manutencoesAbertas,
                ItensEmEstoque = itensEmEstoque,
                UltimasManutencoes = await _context.Manutencoes
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
                    .ToListAsync(),
                GraficoSituacaoLabels = labelsGrafico,
                GraficoSituacaoData = dataGrafico,
                GraficoSituacaoColors = coresGrafico
            };

            return View(viewModel);
        }

        public IActionResult Privacy() => View();

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

      

        
        private static string NormalizeKey(string? input)
        {
            if (string.IsNullOrWhiteSpace(input)) return "desconhecido";
            var formD = input.Normalize(NormalizationForm.FormD);
            var chars = formD
                .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                .ToArray();
            var clean = new string(chars).Normalize(NormalizationForm.FormC);
            return clean.Trim().ToLowerInvariant();
        }

       
        private static string HslFromString(string input)
        {
            using var md5 = MD5.Create();
            var hash = md5.ComputeHash(Encoding.UTF8.GetBytes(input));
           
            var hue = (hash[0] + hash[1] * 256) % 360;
            
            return $"hsl({hue}, 70%, 55%)";
        }
    }
}
