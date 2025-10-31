using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Usuario
{
    public class CriarUsuarioViewModel
    {
        [Required(ErrorMessage = "O Nome é obrigatório")]
        [Display(Name = "Nome Completo")]
        public string NomeCompleto { get; set; }

        [Required(ErrorMessage = "O Email é obrigatório")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "A Senha é obrigatória")]
        [DataType(DataType.Password)]
        public string Senha { get; set; }

        [Required(ErrorMessage = "A Função é obrigatória")]
        [Display(Name = "Função")]
        public string FuncaoId { get; set; } // Vai guardar o NOME da Role
    }
}