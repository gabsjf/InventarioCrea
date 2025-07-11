using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SistemPlanilha.Models;

// Caminho do arquivo: SistemPlanilha/Models/InventarioModel.cs
// Este é o seu modelo correto, agora refletido no Canvas.
namespace SistemPlanilha.Models
{
    public class InventarioModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Editable(false)]
        public int Id { get; set; }

        //[Required(ErrorMessage = "O nome do computador é obrigatório")]
        public string? PcName { get; set; }

       // [Required(ErrorMessage = "O serial é obrigatório")]
        public string? Serial { get; set; }

       // [Required(ErrorMessage = "O patrimônio é obrigatório")]
        public int? Patrimonio { get; set; }

        public string? Usuario { get; set; }
        public string? Modelo { get; set; }
        public string? PrevisaoDevolucao { get; set; }
        public string? Responsavel { get; set; }
        public bool LicencaSO { get; set; }

     
        public bool LicencaOffice { get; set; }

        public string? Processador { get; set; }
        public string? Ssd { get; set; }

        public string? Obs { get; set; }

        // Propriedades de Navegação
        public int? SetorId { get; set; }
        public Setor? Setor { get; set; }

        public int? TipoId { get; set; }
        public Tipo? Tipo { get; set; }

        public int? SituacaoId { get; set; }
        public Situacao? Situacao { get; set; }

        public int? WinVerId { get; set; }
        public WinVer? WinVer { get; set; }

        public int? OfficeId { get; set; }
        public Office? Office { get; set; }
    }
}
