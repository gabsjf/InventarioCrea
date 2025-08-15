using System;
using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.Models
{
    public enum TipoAcao
    {
        Criacao,
        Atualizacao,
        DelecaoLogica
    }

    public class AuditLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Usuario { get; set; }

        [Required]
        public TipoAcao Acao { get; set; }

        [Required]
        public string NomeTabela { get; set; }

        
        [Required]
        public int ChavePrimariaRegistro { get; set; }

        [Required]
        public DateTime Timestamp { get; set; }

        
        public string? Alteracoes { get; set; }
    }
}