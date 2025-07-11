// Arquivo: Models/RelatorioModel.cs

using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.Models
{
    public class RelatorioModel
    {
        public int Id { get; set; }

        // Validação para garantir que um item seja selecionado
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar um item do inventário.")]
        public int InventarioId { get; set; }
        public InventarioModel? Inventario { get; set; }

        [Required(ErrorMessage = "A descrição do problema é obrigatória.")]
        public string Descricao { get; set; }

        public DateTime? DataCriacao { get; set; } = DateTime.Now;

        // O responsável pelo item é preenchido automaticamente, mas o técnico não.
        public string Responsavel { get; set; }

        [Required(ErrorMessage = "O tipo de manutenção é obrigatório.")]
        public string TipoManutencao { get; set; } // preventiva, corretiva etc.

        public string AcoesRealizadas { get; set; }

        public string TempoEstimadoResolucao { get; set; }

        // Validação para garantir que um status seja selecionado
        [Range(1, int.MaxValue, ErrorMessage = "É obrigatório selecionar um status.")]
        public int StatusManutencaoId { get; set; }
        public StatusManutencao? StatusManutencao { get; set; }

        public string ObservacoesAdicionais { get; set; }

        public DateTime? ProximaManutencao { get; set; }

        [Required(ErrorMessage = "O nome do técnico responsável é obrigatório.")]
        public string ResponsavelTecnico { get; set; }

        public string PecasSubstituidas { get; set; }
    }
}