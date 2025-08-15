using System.Threading.Tasks;

namespace SistemPlanilha.Domain.Services
{
    public interface IInventarioService
    {
        Task ValidarPatrimonioParaCriacao(int? patrimonio);
        Task ValidarPatrimonioParaEdicao(int? patrimonio, int itemId);
    }
}