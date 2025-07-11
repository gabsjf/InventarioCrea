using System.Collections.Generic;
using SistemPlanilha.Models;

namespace SistemPlanilha.ViewModels
{
    public class InventarioDetalhesViewModel
    {
        public InventarioModel Inventario { get; set; }
        public List<RelatorioModel> Relatorios { get; set; }
    }
}
