using SistemPlanilha.ViewModels.Manutencao;
using System.Collections.Generic;

namespace SistemPlanilha.ViewModels
{
    public class DashboardViewModel
    {
        public int TotalItensInventario { get; set; }
        public int ManutencoesAbertas { get; set; }
        public int ItensEmEstoque { get; set; }
        public List<RelatorioParaListagemViewModel> UltimasManutencoes { get; set; }

        // PROPRIEDADES NOVAS PARA O GRÁFICO
        public List<string> GraficoSituacaoLabels { get; set; }
        public List<int> GraficoSituacaoData { get; set; }

        public List<string> GraficoSituacaoColors { get; set; }
    }
}