namespace SistemPlanilha.Models
{
    public class StatusManutencao
    {
        public int Id { get; set; }
        public string? Nome { get; set; } 
        public ICollection<RelatorioModel>? Relatorios { get; set; }
    }
}
