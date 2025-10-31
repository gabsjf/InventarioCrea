using System.Collections.Generic;
using SistemPlanilha.Models;

namespace SistemPlanilha.ViewModels.Inventario
{
    public class InventarioDetalhesViewModel
    {
        public InventarioModel Inventario { get; set; }
        public List<ManutencaoModel> Relatorios { get; set; }

        public List<HistoricoSetorModel> HistoricoDeSetores { get; set; }
    }
}
