using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Repositorio;
using System;
using System.Threading.Tasks;

namespace SistemPlanilha.Domain.Services
{
    // Exceção personalizada para patrimônios duplicados
    public class PatrimonioDuplicadoException : Exception
    {
        public PatrimonioDuplicadoException(string message) : base(message) { }
    }

    public class InventarioService : IInventarioService
    {
        private readonly IInventarioRepositorio _inventarioRepositorio;


        private const string TODOS = "todos";

        public InventarioService(IInventarioRepositorio inventarioRepositorio)
        {
            _inventarioRepositorio = inventarioRepositorio;
        }

        public async Task ValidarPatrimonioParaCriacao(int? patrimonio)
        {
            if (!patrimonio.HasValue)
                return;

            if (await PatrimonioJaExiste(patrimonio.Value))
                throw new PatrimonioDuplicadoException(
                    $"O patrimônio número '{patrimonio.Value}' já está cadastrado."
                );
        }

        public async Task ValidarPatrimonioParaEdicao(int? patrimonio, int itemId)
        {
            if (!patrimonio.HasValue)
                return;

            if (await PatrimonioJaExiste(patrimonio.Value, itemId))
                throw new PatrimonioDuplicadoException(
                    $"O patrimônio número '{patrimonio.Value}' já pertence a outro item."
                );
        }

        // Método privado para reaproveitar a consulta
        private async Task<bool> PatrimonioJaExiste(int patrimonio, int? ignorarItemId = null)
        {

            // Criar uma classe para a busca dos filtros e melhorar a leitura dessa parte.
            var query = _inventarioRepositorio.Buscar(null, TODOS, TODOS, null, null, null, null, null);

            if (ignorarItemId.HasValue)
                return await query.AnyAsync(i => i.Patrimonio == patrimonio && i.Id != ignorarItemId.Value);

            return await query.AnyAsync(i => i.Patrimonio == patrimonio);
        }
    }
}
