using System.ComponentModel.DataAnnotations;

namespace SistemPlanilha.ViewModels.Inventario
{
   
    public class EditarInventarioCommand : CriarInventarioCommand
    {
        [Required]
        public int Id { get; set; }
    }
}