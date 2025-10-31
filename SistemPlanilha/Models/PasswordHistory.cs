
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemPlanilha.Models
{
    public class PasswordHistory
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // Chave estrangeira para AspNetUsers(Id)

        [Required]
        public string PasswordHash { get; set; } // O hash da senha antiga

        [Required]
        public DateTime DateCreated { get; set; } // Quando essa senha foi definida

        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; } // Propriedade de navegação
    }
}