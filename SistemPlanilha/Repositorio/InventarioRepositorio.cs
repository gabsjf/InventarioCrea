using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Repositorio
{
    public class InventarioRepositorio : IInventarioRepositorio
    {
        private readonly BancoContext _bancoContext;

        public InventarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public IQueryable<InventarioModel> Buscar(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId)
        {
            // Seu método de busca continua igual, sem alterações.
            IQueryable<InventarioModel> query = _bancoContext.Inventario
                .Include(i => i.WinVer)
                .Include(i => i.Office)
                .Include(i => i.Setor)
                .Include(i => i.Tipo)
                .Include(i => i.Situacao);

            if (!string.IsNullOrWhiteSpace(termo))
            {
                var termoParaBusca = termo.ToLower();
                query = query.Where(x =>
                    (x.PcName != null && x.PcName.ToLower().Contains(termoParaBusca)) ||
                    (x.Usuario != null && x.Usuario.ToLower().Contains(termoParaBusca)) ||
                    (x.Responsavel != null && x.Responsavel.ToLower().Contains(termoParaBusca)) ||
                    (x.Modelo != null && x.Modelo.ToLower().Contains(termoParaBusca)) ||
                    (x.Setor != null && x.Setor.Nome.ToLower().Contains(termoParaBusca)) ||
                    (x.Patrimonio.HasValue && x.Patrimonio.Value.ToString().Contains(termoParaBusca)) ||
                    x.Id.ToString().Contains(termoParaBusca)
                );
            }

            if (filtroSO == "sim") query = query.Where(x => x.LicencaSO);
            else if (filtroSO == "nao") query = query.Where(x => !x.LicencaSO);

            if (filtroOffice == "sim") query = query.Where(x => x.LicencaOffice);
            else if (filtroOffice == "nao") query = query.Where(x => !x.LicencaOffice);

            if (setorId.HasValue) query = query.Where(x => x.SetorId == setorId.Value);
            if (tipoId.HasValue) query = query.Where(x => x.TipoId == tipoId.Value);
            if (situacaoId.HasValue) query = query.Where(x => x.SituacaoId == situacaoId.Value);
            if (winVerId.HasValue) query = query.Where(x => x.WinVerId == winVerId.Value);
            if (officeId.HasValue) query = query.Where(x => x.OfficeId == officeId.Value);

            return query.OrderBy(x => x.Id);
        }

        public async Task<InventarioModel?> ListarPorId(int id)
        {
            return await _bancoContext.Inventario
                .IgnoreQueryFilters()
                .Include(i => i.WinVer).Include(i => i.Office)
                .Include(i => i.Setor).Include(i => i.Tipo).Include(i => i.Situacao)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<InventarioModel> Adicionar(InventarioModel inventario)
        {
            _bancoContext.Inventario.Add(inventario);
            await _bancoContext.SaveChangesAsync();
            return inventario;
        }

        // 👇 MÉTODO ATUALIZAR MODIFICADO
        public async Task Atualizar(InventarioModel inventarioAtualizado, string responsavelPelaAlteracao)
        {
            // 1. Buscamos o estado ATUAL do item no banco, ANTES de qualquer mudança.
            var inventarioDB = await _bancoContext.Inventario
                                                  .AsNoTracking()
                                                  .FirstOrDefaultAsync(i => i.Id == inventarioAtualizado.Id);

            if (inventarioDB == null)
            {
                // Se o item não existe, não fazemos nada. Poderia lançar uma exceção também.
                return;
            }

            // 2. Comparamos o SetorId antigo (do banco) com o novo (que veio do formulário)
            bool setorFoiAlterado = inventarioDB.SetorId.HasValue &&
                                    inventarioAtualizado.SetorId.HasValue &&
                                    inventarioDB.SetorId != inventarioAtualizado.SetorId;

            if (setorFoiAlterado)
            {
                // 3. Se for diferente, criamos o registro de histórico
                var registroHistorico = new HistoricoSetorModel
                {
                    InventarioId = inventarioDB.Id,
                    SetorOrigemId = inventarioDB.SetorId.Value,
                    SetorDestinoId = inventarioAtualizado.SetorId.Value,
                    DataAlteracao = DateTime.UtcNow,
                    ResponsavelAlteracao = responsavelPelaAlteracao
                };
                // 4. Adicionamos o novo registro de histórico ao contexto
                _bancoContext.HistoricoSetores.Add(registroHistorico);
            }

            // 5. Atualizamos o item principal
            _bancoContext.Inventario.Update(inventarioAtualizado);

            // 6. Salvamos TUDO de uma vez (a mudança e o histórico, se houver)
            await _bancoContext.SaveChangesAsync();
        }

        public async Task<bool> Apagar(int id, string usuario)
        {
            var itemParaApagar = await ListarPorId(id);

            if (itemParaApagar != null)
            {
                itemParaApagar.DeletadoEm = DateTime.UtcNow;
                itemParaApagar.DeletadoPor = usuario;

                _bancoContext.Inventario.Update(itemParaApagar);
                await _bancoContext.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}