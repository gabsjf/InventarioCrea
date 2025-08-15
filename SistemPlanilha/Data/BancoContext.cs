using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Models;

namespace SistemPlanilha.Data
{
    public class BancoContext : IdentityDbContext<IdentityUser>
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        // MUDANÇA PRINCIPAL AQUI
        public DbSet<InventarioModel> Inventario { get; set; }

        public DbSet<ManutencaoModel> Manutencoes { get; set; }
        public DbSet<WinVer> WinVer { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Situacao> Situacoes { get; set; }
        public DbSet<StatusManutencao> StatusesManutencao { get; set; }
        public DbSet<AuditLog> Auditoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InventarioModel>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<WinVer>().ToTable("WinVer");
            modelBuilder.Entity<Office>().ToTable("Office");
            modelBuilder.Entity<InventarioModel>().ToTable("Inventario"); 
            modelBuilder.Entity<InventarioModel>().HasQueryFilter(p => p.DeletadoEm == null);
            modelBuilder.Entity<ManutencaoModel>().HasQueryFilter(r => r.DeletadoEm == null);
        }
    }
}