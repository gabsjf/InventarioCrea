namespace SistemPlanilha.Models
{
    public class WinVer : INomeavel
    {
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;

        public ICollection<InventarioModel> Inventarios { get; set; } = new List<InventarioModel>();
    }
}
