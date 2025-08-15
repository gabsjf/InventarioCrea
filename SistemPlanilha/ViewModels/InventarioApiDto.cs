namespace SistemPlanilha.ViewModels.Inventario
{
    public class InventarioApiDto
    {
        public int Id { get; set; }
        public string? PcName { get; set; }
        public string? Usuario { get; set; }
        public int? Patrimonio { get; set; }
        public string? Serial { get; set; }
        public string? Modelo { get; set; }
        public string? Processador { get; set; }
        public string? Ssd { get; set; }
        public string? SetorNome { get; set; }
        public string? TipoNome { get; set; }
        public string? SituacaoNome { get; set; }
        public string? WinVerNome { get; set; }
        public string? OfficeNome { get; set; }
    }
}