using Microsoft.AspNetCore.Mvc.Rendering;
using SistemPlanilha.Data.Helpers; 
using System.Collections.Generic;

namespace SistemPlanilha.ViewModels.Inventario
{
    public class InventarioIndexViewModel
    {
       
        public PaginatedList<InventarioParaListagemViewModel> Inventarios { get; set; }

        public string? TermoAtual { get; set; }
        public string FiltroSO { get; set; }
        public string FiltroOffice { get; set; }
        public int? SetorId { get; set; }
        public int? TipoId { get; set; }
        public int? SituacaoId { get; set; }
        public int? WinVerId { get; set; }
        public int? OfficeId { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public SelectList? Setores { get; set; }
        public SelectList? Tipos { get; set; }
        public SelectList? Situacoes { get; set; }
        public SelectList? WinVers { get; set; }
        public SelectList? Offices { get; set; }
    }
}