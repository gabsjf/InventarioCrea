using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Conta
{
    public class MudarSenhaPrimeiroAcesso
    {
        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [MinLength(6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}