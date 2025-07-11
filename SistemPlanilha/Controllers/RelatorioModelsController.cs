using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using SistemPlanilha.ViewModels;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    public class RelatorioModelsController : Controller
    {
        private readonly BancoContext _context;

        public RelatorioModelsController(BancoContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? inventarioId)
        {
            IQueryable<RelatorioModel> query = _context.RelatoriosManutencao
                .Include(r => r.Inventario)
                .Include(r => r.StatusManutencao);

            if (inventarioId.HasValue)
            {
                query = query.Where(r => r.InventarioId == inventarioId.Value);
            }

            return View(await query.ToListAsync());
        }

   
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var relatorioModel = await _context.RelatoriosManutencao
                .Include(r => r.Inventario)
                .Include(r => r.StatusManutencao)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relatorioModel == null) return NotFound();
            return View(relatorioModel);
        }


        public IActionResult Create(int? inventarioId)
        {
            string nomeItem = string.Empty;
            string responsavelItem = string.Empty;

            if (inventarioId.HasValue)
            {
                var inventario = _context.Inventario.FirstOrDefault(i => i.Id == inventarioId.Value);
                if (inventario != null)
                {
                    nomeItem = inventario.PcName;
                    responsavelItem = inventario.Usuario;
                }
            }
            ViewBag.NomeItem = nomeItem;

            var viewModel = new RelatorioFormViewModel
            {
                Relatorio = new RelatorioModel
                {
                    InventarioId = inventarioId ?? 0,
                    Responsavel = responsavelItem
                },
                InventarioItens = new SelectList(_context.Inventario.OrderBy(i => i.PcName), "Id", "PcName", inventarioId),
                StatusesManutencao = new SelectList(_context.StatusesManutencao, "Id", "Nome")
            };

            return View(viewModel);
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RelatorioFormViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                viewModel.Relatorio.DataCriacao = DateTime.Now;
                _context.Add(viewModel.Relatorio);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index)); // Redireciona para a lista completa
            }

            viewModel.InventarioItens = new SelectList(_context.Inventario.OrderBy(i => i.PcName), "Id", "PcName", viewModel.Relatorio.InventarioId);
            viewModel.StatusesManutencao = new SelectList(_context.StatusesManutencao, "Id", "Nome", viewModel.Relatorio.StatusManutencaoId);
            return View(viewModel);
        }

    
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var relatorioModel = await _context.RelatoriosManutencao.FindAsync(id);
            if (relatorioModel == null) return NotFound();

            var viewModel = new RelatorioFormViewModel
            {
                Relatorio = relatorioModel,
                InventarioItens = new SelectList(_context.Inventario.OrderBy(i => i.PcName), "Id", "PcName", relatorioModel.InventarioId),
                StatusesManutencao = new SelectList(_context.StatusesManutencao, "Id", "Nome", relatorioModel.StatusManutencaoId)
            };

            return View(viewModel);
        }

        // POST: RelatorioModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, RelatorioFormViewModel viewModel)
        {
            if (id != viewModel.Relatorio.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(viewModel.Relatorio);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RelatorioModelExists(viewModel.Relatorio.Id)) return NotFound();
                    else throw;
                }
                // --- CORREÇÃO APLICADA AQUI ---
                return RedirectToAction(nameof(Index)); // Redireciona para a lista completa
            }

            viewModel.InventarioItens = new SelectList(_context.Inventario.OrderBy(i => i.PcName), "Id", "PcName", viewModel.Relatorio.InventarioId);
            viewModel.StatusesManutencao = new SelectList(_context.StatusesManutencao, "Id", "Nome", viewModel.Relatorio.StatusManutencaoId);
            return View(viewModel);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var relatorioModel = await _context.RelatoriosManutencao
                .Include(r => r.Inventario)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (relatorioModel == null) return NotFound();
            return View(relatorioModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var relatorioModel = await _context.RelatoriosManutencao.FindAsync(id);
            if (relatorioModel != null)
            {
                _context.RelatoriosManutencao.Remove(relatorioModel);
                await _context.SaveChangesAsync();
            }
            
            return RedirectToAction(nameof(Index)); // Redireciona para a lista completa
        }

        private bool RelatorioModelExists(int id)
        {
            return _context.RelatoriosManutencao.Any(e => e.Id == id);
        }
    }
}