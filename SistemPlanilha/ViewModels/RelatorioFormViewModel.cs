using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemPlanilha.Models;

namespace SistemPlanilha.ViewModels
{
    public class RelatorioFormViewModel
    {
        public RelatorioModel Relatorio { get; set; }

        
        [BindNever]
        public SelectList? InventarioItens { get; set; }

       
        [BindNever]
        public SelectList? StatusesManutencao { get; set; }
    }
}