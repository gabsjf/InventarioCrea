namespace SistemPlanilha.Models
{
    public class Tipo : INomeavel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public ICollection<InventarioModel>? Inventarios { get; set; }
    }
}
