using System.ComponentModel.DataAnnotations;

// Garanta que este namespace corresponde à estrutura do seu projeto
namespace SistemPlanilha.ViewModels.Conta
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "A senha atual é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        [Required(ErrorMessage = "A nova senha é obrigatória.")]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        [MinLength(8, ErrorMessage = "A senha deve ter no mínimo 8 caracteres.")] // Garante que a validação bate com as regras do Identity
        // Adicione aqui outros atributos se precisar validar a complexidade diretamente no ViewModel
        // [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^a-zA-Z\d]).{8,}$", ErrorMessage = "A senha deve conter maiúscula, minúscula, número e caractere especial.")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a confirmação não coincidem.")]
        public string ConfirmPassword { get; set; }
    }
}