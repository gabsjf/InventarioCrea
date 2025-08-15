using SistemPlanilha.Models;
using SistemPlanilha.ViewModels.Inventario;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public interface IInventarioApp
    {
        Task<InventarioIndexViewModel> ObterDadosPaginaIndex(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId, int page, int pageSize);
        Task<InventarioDetalhesViewModel?> ObterDetalhesInventario(int id);
        Task<ExibirInventarioFormViewModel> PrepararViewModelParaCriar();
        Task AdicionarInventario(CriarInventarioCommand command);
        Task<ExibirInventarioEditarFormViewModel?> PrepararViewModelParaEditar(int id);
        Task AtualizarInventario(EditarInventarioCommand command);
        Task ApagarInventario(int id);
        Task<InventarioApiDto?> ObterInventarioDtoPorId(int id);
        Task<InventarioModel?> ObterModelPorId(int id);
    }
}