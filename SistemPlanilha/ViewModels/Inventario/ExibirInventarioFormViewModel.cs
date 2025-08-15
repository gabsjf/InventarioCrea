using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemPlanilha.ViewModels.Inventario
{
   
    public class ExibirInventarioFormViewModel
    {
        
        public CriarInventarioCommand Command { get; set; }

        
        public SelectList? Setores { get; set; }
        public SelectList? Tipos { get; set; }
        public SelectList? Situacoes { get; set; }
        public SelectList? WinVers { get; set; }
        public SelectList? Offices { get; set; }

        public ExibirInventarioFormViewModel()
        {
            
            Command = new CriarInventarioCommand();
        }
    }
}