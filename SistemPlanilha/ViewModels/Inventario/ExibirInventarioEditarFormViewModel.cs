using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemPlanilha.ViewModels.Inventario
{
    public class ExibirInventarioEditarFormViewModel
    {
        public EditarInventarioCommand Command { get; set; }

        public SelectList? Setores { get; set; }
        public SelectList? Tipos { get; set; }
        public SelectList? Situacoes { get; set; }
        public SelectList? WinVers { get; set; }
        public SelectList? Offices { get; set; }

        public ExibirInventarioEditarFormViewModel()
        {
            Command = new EditarInventarioCommand();
        }
    }
}