using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using System;
using System.Linq;

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
           
            IQueryable<InventarioModel> query = _bancoContext.Inventario
                                                              .Include(i => i.WinVer)
                                                              .Include(i => i.Office)
                                                              .Include(i => i.Setor)
                                                              .Include(i => i.Tipo)
                                                              .Include(i => i.Situacao);

           
            var termoParaBusca = termo?.ToLower() ?? string.Empty;
            var termoLike = $"%{termoParaBusca}%";

            if (!string.IsNullOrWhiteSpace(termoParaBusca)) 
            {
                query = query.Where(x =>
                    (x.PcName != null && EF.Functions.Like(x.PcName.ToLower(), termoLike)) ||
                    (x.Usuario != null && EF.Functions.Like(x.Usuario.ToLower(), termoLike)) ||
                    (x.Responsavel != null && EF.Functions.Like(x.Responsavel.ToLower(), termoLike)) ||
                    (x.Processador != null && EF.Functions.Like(x.Processador.ToLower(), termoLike)) ||
                    (x.Ssd != null && EF.Functions.Like(x.Ssd.ToLower(), termoLike)) ||
                    (x.Obs != null && EF.Functions.Like(x.Obs.ToLower(), termoLike)) ||
                    (x.Modelo != null && EF.Functions.Like(x.Modelo.ToLower(), termoLike)) ||
                    (x.Setor != null && x.Setor.Nome != null && EF.Functions.Like(x.Setor.Nome.ToLower(), termoLike)) || 
                    (x.Tipo != null && x.Tipo.Nome != null && EF.Functions.Like(x.Tipo.Nome.ToLower(), termoLike)) ||    
                    (x.Situacao != null && x.Situacao.Nome != null && EF.Functions.Like(x.Situacao.Nome.ToLower(), termoLike)) || 
                    (x.WinVer != null && x.WinVer.Nome != null && EF.Functions.Like(x.WinVer.Nome.ToLower(), termoLike)) ||
                    (x.Office != null && x.Office.Nome != null && EF.Functions.Like(x.Office.Nome.ToLower(), termoLike)) ||
                    (x.Patrimonio != null && EF.Functions.Like(x.Patrimonio.ToString(), termoLike)) ||
                    EF.Functions.Like(x.Id.ToString(), termoLike)
                );
            }

            if (filtroSO == "sim") query = query.Where(x => x.LicencaSO);
            else if (filtroSO == "nao") query = query.Where(x => !x.LicencaSO);

            if (filtroOffice == "sim") query = query.Where(x => x.LicencaOffice);
            else if (filtroOffice == "nao") query = query.Where(x => !x.LicencaOffice);

            if (setorId.HasValue && setorId > 0) query = query.Where(x => x.SetorId == setorId.Value);
            if (tipoId.HasValue && tipoId > 0) query = query.Where(x => x.TipoId == tipoId.Value);
            if (situacaoId.HasValue && situacaoId > 0) query = query.Where(x => x.SituacaoId == situacaoId.Value);

      
            if (winVerId.HasValue && winVerId > 0) query = query.Where(x => x.WinVerId == winVerId.Value);
            if (officeId.HasValue && officeId > 0) query = query.Where(x => x.OfficeId == officeId.Value);

            
            return query.OrderBy(x => x.Id);
        }

        public InventarioModel ListarPorId(int id)
        {
            return _bancoContext.Inventario
                .Include(i => i.WinVer)
                .Include(i => i.Office)
                .Include(i => i.Setor)
                .Include(i => i.Tipo)
                .Include(i => i.Situacao)
                .FirstOrDefault(x => x.Id == id);
        }

        public bool Apagar(int id)
        {
            InventarioModel inventarioDB = ListarPorId(id);
            if (inventarioDB == null) throw new System.Exception("Erro na deleção do item do inventário.");

            _bancoContext.Inventario.Remove(inventarioDB);
            _bancoContext.SaveChanges();
            return true;
        }

        public InventarioModel Atualizar(InventarioModel inventario)
        {
            InventarioModel inventarioDB = ListarPorId(inventario.Id);
            if (inventarioDB == null)
            {
                throw new Exception("Houve um erro na atualização do inventário: item não encontrado.");
            }

            
            inventarioDB.PcName = inventario.PcName;
            inventarioDB.Serial = inventario.Serial;
            inventarioDB.Patrimonio = inventario.Patrimonio;
            inventarioDB.Usuario = inventario.Usuario;
            inventarioDB.Modelo = inventario.Modelo;
            inventarioDB.PrevisaoDevolucao = inventario.PrevisaoDevolucao;
            inventarioDB.Responsavel = inventario.Responsavel;
            inventarioDB.LicencaSO = inventario.LicencaSO;
            inventarioDB.LicencaOffice = inventario.LicencaOffice;
            inventarioDB.Processador = inventario.Processador;
            inventarioDB.Ssd = inventario.Ssd;
            inventarioDB.Obs = inventario.Obs;
            inventarioDB.SetorId = inventario.SetorId;
            inventarioDB.TipoId = inventario.TipoId;
            inventarioDB.SituacaoId = inventario.SituacaoId;
            inventarioDB.WinVerId = inventario.WinVerId;
            inventarioDB.OfficeId = inventario.OfficeId;

            _bancoContext.Inventario.Update(inventarioDB);
            _bancoContext.SaveChanges();

            return inventarioDB;
        }

        public InventarioModel Adicionar(InventarioModel inventario)
        {
            _bancoContext.Inventario.Add(inventario); // CORREÇÃO: Use _bancoContext.Inventario
            _bancoContext.SaveChanges();
            return inventario;
        }
    }
}