using SistemPlanilha.Models; // Necessário para usar o enum TipoAcao
using System;

namespace SistemPlanilha.ViewModels.Usuario
{
    public class AuditLogViewModel
    {
        public DateTime Timestamp { get; set; }
        public TipoAcao Acao { get; set; }
        public string NomeTabela { get; set; }
        public int ChavePrimariaRegistro { get; set; }

        public string DescricaoRegistro { get; set; }
        public int InventarioId { get; set; }
    }
}