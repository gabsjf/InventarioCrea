// Em Models/HistoricoSetorModel.cs

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemPlanilha.Models
{
    public class HistoricoSetorModel
    {
        [Key]
        public int Id { get; set; }

        // Chave estrangeira para o item de inventário que foi alterado
        [Required]
        public int InventarioId { get; set; }
        public InventarioModel Inventario { get; set; }

        // Chave estrangeira para o setor de onde o item saiu
        [Required]
        public int SetorOrigemId { get; set; }
        [ForeignKey("SetorOrigemId")]
        public Setor SetorOrigem { get; set; }

        // Chave estrangeira para o setor para onde o item foi
        [Required]
        public int SetorDestinoId { get; set; }
        [ForeignKey("SetorDestinoId")]
        public Setor SetorDestino { get; set; }

        // Quem fez a alteração?
        [Required]
        public string ResponsavelAlteracao { get; set; }

        // Quando a alteração ocorreu?
        [Required]
        public DateTime DataAlteracao { get; set; }
    }
}