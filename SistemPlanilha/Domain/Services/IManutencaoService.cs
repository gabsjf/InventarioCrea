using System.Threading.Tasks;

namespace SistemPlanilha.Domain.Services
{
    public interface IManutencaoService
    {
        Task ValidarInventarioParaNovaManutencao(int inventarioId);
    }
}