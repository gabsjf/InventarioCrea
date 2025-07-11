// Em /ViewModels/InventarioFormViewModel.cs
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // Adicione este using se for usar IEnumerable<SelectListItem>

namespace SistemPlanilha.ViewModels
{
    public class InventarioFormViewModel
    {
        // --- Propriedades de Identificação e Dados Principais ---
        public int Id { get; set; } // Usado para edição, se aplicável

        [Display(Name = "Nome do Computador")]
        [Required(ErrorMessage = "O nome do computador é obrigatório.")] 
        public string? PcName { get; set; }

        [Display(Name = "Serial")]
        public string? Serial { get; set; }

        [Display(Name = "Patrimônio")]
        public int? Patrimonio { get; set; }

        [Display(Name = "Usuário")]
        public string? Usuario { get; set; }

        [Display(Name = "Modelo")]
        public string? Modelo { get; set; }

        [Display(Name = "Responsável")]
        public string? Responsavel { get; set; }

        [Display(Name = "Processador")]
        public string? Processador { get; set; }

        [Display(Name = "SSD")]
        public string? Ssd { get; set; }

        [Display(Name = "Observações")]
        public string? Obs { get; set; }

        [Display(Name = "S.O Licenciado")]
        public bool LicencaSO { get; set; }

        [Display(Name = "Office Licenciado")]
        public bool LicencaOffice { get; set; }

        
        [Display(Name = "Setor")]
        [Required(ErrorMessage = "O setor é obrigatório.")]
        public int? SetorId { get; set; }

        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O tipo é obrigatório.")]
        public int? TipoId { get; set; }

        [Display(Name = "Situação")]
        [Required(ErrorMessage = "A situação é obrigatória.")]
        public int? SituacaoId { get; set; }

        [Display(Name = "Versão do Windows")]
        [Required(ErrorMessage = "A versão do Windows é obrigatória.")]
        public int? WinVerId { get; set; }

        [Display(Name = "Versão do Office")]
        [Required(ErrorMessage = "A versão do Office é obrigatória.")]
        public int? OfficeId { get; set; }

      
        [BindNever]
        public SelectList? Setores { get; set; }
        [BindNever]
        public SelectList? Tipos { get; set; }
        [BindNever]
        public SelectList? Situacoes { get; set; }
        [BindNever]
        public SelectList? WinVers { get; set; }
        [BindNever]
        public SelectList? Offices { get; set; }
    }
}