using System;

namespace SistemPlanilha.ViewModels.Manutencao // <-- Namespace corrigido
{
    public class RelatorioDetalhesViewModel
    {
        public int Id { get; set; }
        public int InventarioId { get; set; }
        public string? InventarioPcName { get; set; }
        public string? Descricao { get; set; }
        public string? DataCriacao { get; set; }
        public string? Responsavel { get; set; }
        public string? TipoManutencao { get; set; }
        public string? AcoesRealizadas { get; set; }
        public string? TempoEstimadoResolucao { get; set; }
        public string? StatusNome { get; set; }
        public string? ObservacoesAdicionais { get; set; }
        public string? ProximaManutencao { get; set; }
        public string? ResponsavelTecnico { get; set; }
        public string? PecasSubstituidas { get; set; }
    }
}