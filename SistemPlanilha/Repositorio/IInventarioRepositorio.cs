using SistemPlanilha.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Repositorio
{
    public interface IInventarioRepositorio
    {
        Task<InventarioModel> ListarPorId(int id);
        IQueryable<InventarioModel> Buscar(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId);
        Task<InventarioModel> Adicionar(InventarioModel inventario);
        Task Atualizar(InventarioModel inventario);

        // MUDANÇA AQUI
        Task<bool> Apagar(int id, string usuario);
    }
}