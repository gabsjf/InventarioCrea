using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Inventario
{
    public class CriarInventarioCommand
    {
        [Display(Name = "Nome do Computador")]
        [Required(ErrorMessage = "O nome do computador é obrigatório.")]
        public string? PcName { get; set; }

        public string? Serial { get; set; }
        public int? Patrimonio { get; set; }
        public string? Usuario { get; set; }
        public string? Modelo { get; set; }
        public string? PrevisaoDevolucao { get; set; }
        public string? Responsavel { get; set; }
        public string? Processador { get; set; }
        public string? Ssd { get; set; }
        public string? Obs { get; set; }

        [Display(Name = "S.O Licenciado")]
        public bool LicencaSO { get; set; } 

        [Display(Name = "Office Licenciado")]
        public bool LicencaOffice { get; set; }

        [Display(Name = "Setor")]
        [Required(ErrorMessage = "O setor é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O setor é obrigatório.")]
        public int? SetorId { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O tipo é obrigatório.")]
        public int? TipoId { get; set; }

        [Display(Name = "Situação")]
        [Required(ErrorMessage = "A situação é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A situação é obrigatória.")]
        public int? SituacaoId { get; set; }

        [Display(Name = "Versão do Windows")]
        [Required(ErrorMessage = "A versão do Windows é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A versão do Windows é obrigatória.")]
        public int? WinVerId { get; set; }

        [Display(Name = "Versão do Office")]
        [Required(ErrorMessage = "A versão do Office é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A versão do Office é obrigatória.")]
        public int? OfficeId { get; set; }
    }
}