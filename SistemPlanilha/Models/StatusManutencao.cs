namespace SistemPlanilha.Models
{
    public class StatusManutencao
    {
        public int Id { get; set; }
        public string? Nome { get; set; }  // Exemplo: "Pendente", "Em andamento", "Concluído"

        public ICollection<RelatorioModel>? Relatorios { get; set; }
    }
}
