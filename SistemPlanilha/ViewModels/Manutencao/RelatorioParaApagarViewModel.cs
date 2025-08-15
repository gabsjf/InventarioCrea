namespace SistemPlanilha.ViewModels.Manutencao // <-- Namespace corrigido
{
    public class RelatorioParaApagarViewModel
    {
        public int Id { get; set; }
        public int InventarioId { get; set; }
        public string? Descricao { get; set; }
        public string? ResponsavelTecnico { get; set; }
    }
}