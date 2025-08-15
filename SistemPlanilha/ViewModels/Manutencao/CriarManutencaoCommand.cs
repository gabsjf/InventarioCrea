using System;
using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Manutencao // <-- Namespace corrigido
{
    public class CriarManutencaoCommand
    {
        [Display(Name = "Item do Inventário")]
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar um item.")]
        public int InventarioId { get; set; }

        [Display(Name = "Descrição do Problema")]
        [Required(ErrorMessage = "A descrição é obrigatória.")]
        public string? Descricao { get; set; }

        [Display(Name = "Status da Manutenção")]
        [Range(1, int.MaxValue, ErrorMessage = "O status é obrigatório.")]
        public int StatusManutencaoId { get; set; }

        [Display(Name = "Responsável pelo Item")]
        public string? Responsavel { get; set; }

        [Display(Name = "Técnico Responsável")]
        [Required(ErrorMessage = "O técnico é obrigatório.")]
        public string? ResponsavelTecnico { get; set; }

        [Display(Name = "Ações Realizadas")]
        public string? AcoesRealizadas { get; set; }

        [Display(Name = "Tipo de Manutenção")]
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        public string? TipoManutencao { get; set; }

        [Display(Name = "Tempo Estimado")]
        public string? TempoEstimadoResolucao { get; set; }

        [Display(Name = "Peças Substituídas")]
        public string? PecasSubstituidas { get; set; }

        [Display(Name = "Observações Adicionais")]
        public string? ObservacoesAdicionais { get; set; }

        [Display(Name = "Próxima Manutenção")]
        public DateTime? ProximaManutencao { get; set; }
    }
}