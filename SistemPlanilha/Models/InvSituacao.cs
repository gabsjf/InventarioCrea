namespace SistemPlanilha.Models
{
    public class Situacao : INomeavel
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public ICollection<InventarioModel>? Inventarios { get; set; }
    }
}
