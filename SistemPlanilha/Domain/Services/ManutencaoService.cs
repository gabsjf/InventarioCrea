using SistemPlanilha.Repositorio;
using System;
using System.Threading.Tasks;

namespace SistemPlanilha.Domain.Services
{
    public class ManutencaoService : IManutencaoService
    {
        private readonly IInventarioRepositorio _inventarioRepositorio;

        public ManutencaoService(IInventarioRepositorio inventarioRepositorio)
        {
            _inventarioRepositorio = inventarioRepositorio;
        }

        public async Task ValidarInventarioParaNovaManutencao(int inventarioId)
        {
            var inventario = await _inventarioRepositorio.ListarPorId(inventarioId);

            if (inventario == null)
            {
                throw new Exception($"O item de inventário com ID '{inventarioId}' não foi encontrado.");
            }

            if (inventario.DeletadoEm.HasValue)
            {
                throw new Exception($"Não é possível criar uma manutenção para o item '{inventario.PcName}', pois ele foi deletado em {inventario.DeletadoEm:dd/MM/yyyy}.");
            }
        }
    }
}