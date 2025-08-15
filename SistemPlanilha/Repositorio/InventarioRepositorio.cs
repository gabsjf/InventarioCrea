using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Repositorio
{
    // A "casca" da classe que estava faltando
    public class InventarioRepositorio : IInventarioRepositorio
    {
        private readonly BancoContext _bancoContext;

        public InventarioRepositorio(BancoContext bancoContext)
        {
            _bancoContext = bancoContext;
        }

        public IQueryable<InventarioModel> Buscar(string termo, string filtroSO, string filtroOffice, int? setorId, int? tipoId, int? situacaoId, int? winVerId, int? officeId)
        {
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

        public async Task Atualizar(InventarioModel inventario)
        {
            _bancoContext.Inventario.Update(inventario);
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