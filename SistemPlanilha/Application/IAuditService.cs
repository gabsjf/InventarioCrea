using SistemPlanilha.Models;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public interface IAuditService
    {
        Task RegistrarAuditoria(TipoAcao acao, string nomeTabela, int chavePrimaria, string usuario, object? dadosAlterados = null);
    }
}