namespace SistemPlanilha.Models
{
    public class Setor : INomeavel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public ICollection<InventarioModel>? Inventarios { get; set; }
    }
}
