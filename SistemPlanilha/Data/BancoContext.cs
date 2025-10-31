using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Models;

namespace SistemPlanilha.Data
{
    public class BancoContext : IdentityDbContext<ApplicationUser>
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        public DbSet<InventarioModel> Inventario { get; set; }
        public DbSet<ManutencaoModel> Manutencoes { get; set; } // Note: Assuming this is the correct name for RelatorioModel
        public DbSet<WinVer> WinVer { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Situacao> Situacoes { get; set; }
        public DbSet<StatusManutencao> StatusesManutencao { get; set; }
        public DbSet<AuditLog> Auditoria { get; set; } // Note: Assuming this exists
        public DbSet<HistoricoSetorModel> HistoricoSetores { get; set; }

        public DbSet<PasswordHistory> PasswordHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurações existentes
            modelBuilder.Entity<InventarioModel>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WinVer>().ToTable("WinVer");
            modelBuilder.Entity<Office>().ToTable("Office");
            modelBuilder.Entity<InventarioModel>().ToTable("Inventario");

            // Filtros para Delete Lógico
            modelBuilder.Entity<InventarioModel>().HasQueryFilter(p => p.DeletadoEm == null);
            // Assuming ManutencaoModel also has a soft delete property
            // modelBuilder.Entity<ManutencaoModel>().HasQueryFilter(r => r.DeletadoEm == null);

            //  CONFIGURAÇÃO PARA CORRIGIR O ERRO "MULTIPLE CASCADE PATHS"
            // Define o comportamento ao deletar para as chaves estrangeiras da tabela de histórico
            modelBuilder.Entity<HistoricoSetorModel>()
                .HasOne(h => h.SetorOrigem)
                .WithMany() // Relação sem propriedade de navegação de volta em 'Setor'
                .HasForeignKey(h => h.SetorOrigemId)
                .OnDelete(DeleteBehavior.Restrict); // Impede o cascade delete

            modelBuilder.Entity<HistoricoSetorModel>()
                .HasOne(h => h.SetorDestino)
                .WithMany()
                .HasForeignKey(h => h.SetorDestinoId)
                .OnDelete(DeleteBehavior.Restrict); // Impede o cascade delete
            
            }

    }
}