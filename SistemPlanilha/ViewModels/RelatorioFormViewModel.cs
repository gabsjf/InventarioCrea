using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using SistemPlanilha.Models;

namespace SistemPlanilha.ViewModels
{
    public class RelatorioFormViewModel
    {
        public RelatorioModel Relatorio { get; set; }

        // Atributo para dizer ao sistema: "Não tente validar ou preencher esta propriedade no POST"
        [BindNever]
        public SelectList? InventarioItens { get; set; }

        // O mesmo aqui
        [BindNever]
        public SelectList? StatusesManutencao { get; set; }
    }
}