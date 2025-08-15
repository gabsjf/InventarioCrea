using Microsoft.AspNetCore.Mvc.Rendering;

namespace SistemPlanilha.ViewModels.Manutencao
{
    public class ExibirManutencaoFormViewModel
    {
        public EditarManutencaoCommand Command { get; set; }
        public SelectList? InventarioItens { get; set; }
        public SelectList? StatusesManutencao { get; set; }

        public ExibirManutencaoFormViewModel()
        {
            Command = new EditarManutencaoCommand();
        }
    }
}