using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Models;

namespace SistemPlanilha.Data
{
    public class BancoContext : DbContext
    {
        public BancoContext(DbContextOptions<BancoContext> options) : base(options) { }

        // DbSets (ENTIDADES/TABELAS)
        public DbSet<InventarioModel> Inventario { get; set; }
        public DbSet<WinVer> WinVer { get; set; }
        public DbSet<Tipo> Tipos { get; set; }
        public DbSet<Office> Office { get; set; }
        public DbSet<Setor> Setores { get; set; }
        public DbSet<Situacao> Situacoes { get; set; }
        public DbSet<RelatorioModel> RelatoriosManutencao { get; set; }
        public DbSet<StatusManutencao> StatusesManutencao { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Desativa o auto incremento do ID
            modelBuilder.Entity<InventarioModel>()
                .Property(i => i.Id)
                .ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<WinVer>().ToTable("WinVer"); 
            modelBuilder.Entity<Office>().ToTable("Office");

        }
    }
}
