using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SistemPlanilha.Models;


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
        public Setor? Setor { get; private set; }

        public int? TipoId { get; set; }
        public Tipo? Tipo { get; set; }

        public int? SituacaoId { get; set; }
        public Situacao? Situacao { get; set; }

        public int? WinVerId { get; set; }
        public WinVer? WinVer { get; set; }

        public int? OfficeId { get; set; }
        public Office? Office { get; set; }

        // campo para controle de Delete lógico no banco de dados
        public DateTime? DeletadoEm { get; set; }
        public string? DeletadoPor { get; set; }

        public string? CriadoPor { get; set; }
        public DateTime? DataAtualizacao { get; set; }
        public string? AtualizadoPor { get; set; }
        public DateTime DataCriacao { get; set; }

    }
}
