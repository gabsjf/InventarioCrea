using SistemPlanilha.Models;

namespace SistemPlanilha.ViewModels.Inventario
{

    public class InventarioApagarViewModel
    {

        public int Id { get; set; }

        public string PcName { get; set; }

        public string Serial { get; set; }
        public int? Patrimonio { get; set; }
    }
}