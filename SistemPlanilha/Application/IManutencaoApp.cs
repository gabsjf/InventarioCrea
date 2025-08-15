using SistemPlanilha.ViewModels.Manutencao;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public interface IManutencaoApp
    {
        Task<IEnumerable<RelatorioParaListagemViewModel>> ObterTodosParaListagem(int? inventarioId);
        Task<RelatorioDetalhesViewModel?> ObterDetalhes(int id);
        Task<ExibirManutencaoFormViewModel> PrepararViewModelParaCriar(int? inventarioId);

        // Nomes corrigidos para "Manutencao"
        Task CriarManutencao(CriarManutencaoCommand command);
        Task<ExibirManutencaoFormViewModel?> PrepararViewModelParaEditar(int id);
        Task AtualizarManutencao(EditarManutencaoCommand command);
        Task<RelatorioParaApagarViewModel?> ObterDadosParaApagar(int id);
        Task DeletarManutencaoLogicamente(int id);
    }
}