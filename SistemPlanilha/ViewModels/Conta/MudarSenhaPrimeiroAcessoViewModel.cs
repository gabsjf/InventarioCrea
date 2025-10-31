using System.ComponentModel.DataAnnotations;

// Garanta que este namespace corresponde à estrutura do seu projeto
namespace SistemPlanilha.ViewModels.Conta
{
    public class MudarSenhaPrimeiroAcessoViewModel
    {
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")] // Garante que a validação bate com as regras do Identity
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}