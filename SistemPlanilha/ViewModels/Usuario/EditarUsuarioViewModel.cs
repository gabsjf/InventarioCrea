using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Usuario
{
    public class EditarUsuarioViewModel
    {
        [Required]
        public string Id { get; set; }

        [Display(Name = "Nome Completo")]
        [Required(ErrorMessage = "O Nome é obrigatório")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Função")]
        public string Funcao { get; set; }

        public SelectList? TodasAsFuncoes { get; set; }
    }
}