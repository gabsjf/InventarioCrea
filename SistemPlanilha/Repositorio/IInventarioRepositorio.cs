using SistemPlanilha.Models;
using System.Linq;

namespace SistemPlanilha.Repositorio
{
    public interface IInventarioRepositorio
    {
        InventarioModel ListarPorId(int id);
        IQueryable<InventarioModel> Buscar(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId);
        InventarioModel Adicionar(InventarioModel inventario);
        InventarioModel Atualizar(InventarioModel inventario);
        bool Apagar(int id);
    }
}