using System;
using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.Models
{
    public class ManutencaoModel
    {
        public int Id { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar um item do inventário.")]
        public int InventarioId { get; set; }
        public InventarioModel? Inventario { get; set; }

        [Required(ErrorMessage = "A descrição do problema é obrigatória.")]
        public string Descricao { get; set; }

        public string Responsavel { get; set; }

        [Required(ErrorMessage = "O tipo de manutenção é obrigatório.")]
        public string TipoManutencao { get; set; }

        public string AcoesRealizadas { get; set; }

        public string TempoEstimadoResolucao { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar um status.")]
        public int StatusManutencaoId { get; set; }
        public StatusManutencao? StatusManutencao { get; set; }

        public string? ObservacoesAdicionais { get; set; }

        // GARANTINDO QUE SÓ EXISTA ESTA DECLARAÇÃO
        public DateTime? ProximaManutencao { get; set; }

        [Required(ErrorMessage = "O nome do técnico responsável é obrigatório.")]
        public string? ResponsavelTecnico { get; set; }

        public string? PecasSubstituidas { get; set; }

        // --- CAMPOS DE AUDITORIA COMPLETOS ---
        public DateTime DataCriacao { get; set; }
        public string? CriadoPor { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string? AtualizadoPor { get; set; }
        public DateTime? DeletadoEm { get; set; }
        public string? DeletadoPor { get; set; }
    }
}