using Microsoft.AspNetCore.Identity;

namespace SistemPlanilha.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string NomeCompleto { get; set; }
        public bool MudarSenhaPrimeiroAcesso { get; set; } = true;
        public DateTime? PasswordLastChangedDate { get; set; }
    }
}