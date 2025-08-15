using System;
using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Manutencao // <-- Namespace corrigido
{
    public class EditarManutencaoCommand : CriarManutencaoCommand
    {
        [Required]
        public int Id { get; set; }
    }
}