using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data;
using SistemPlanilha.Models;
using SistemPlanilha.ViewModels.Usuario;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    [Authorize(Roles = "Administrador")] // Apenas Admins podem acessar esta área
    public class UsuarioController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly BancoContext _context;

        public UsuarioController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            BancoContext context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var usuarios = await _userManager.Users.ToListAsync();
            var viewModel = new List<UsuarioListagemViewModel>();
            foreach (var usuario in usuarios)
            {
                var funcoes = await _userManager.GetRolesAsync(usuario);
                viewModel.Add(new UsuarioListagemViewModel
                {
                    Id = usuario.Id,
                    NomeCompleto = usuario.NomeCompleto,
                    Email = usuario.Email,
                    Funcao = funcoes.FirstOrDefault() ?? "Sem Função"
                });
            }
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Detalhes(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var logs = await _context.Auditoria
                .Where(log => log.Usuario == usuario.Email)
                .OrderByDescending(log => log.Timestamp)
                .ToListAsync();

            var viewModel = new UsuarioDetalhesViewModel
            {
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
            };

            // Lógica para "enriquecer" os logs com a descrição E o InventarioId
            foreach (var log in logs)
            {
                string descricao = $"ID do Registro: {log.ChavePrimariaRegistro}";
                int inventarioId = 0;

                if (log.NomeTabela == "Inventarios")
                {
                    var item = await _context.Inventario.IgnoreQueryFilters()
                                           .FirstOrDefaultAsync(i => i.Id == log.ChavePrimariaRegistro);
                    if (item != null)
                    {
                        descricao = item.PcName;
                        inventarioId = item.Id; // O ID do inventário é o próprio ID do registro
                    }
                }
                else if (log.NomeTabela == "Manutencoes")
                {
                    var item = await _context.Manutencoes.IgnoreQueryFilters()
                                           .FirstOrDefaultAsync(m => m.Id == log.ChavePrimariaRegistro);
                    if (item != null)
                    {
                        descricao = item.Descricao;
                        inventarioId = item.InventarioId; // Pegamos o ID do inventário associado
                    }
                }

                viewModel.LogsDeAtividade.Add(new AuditLogViewModel
                {
                    Timestamp = log.Timestamp,
                    Acao = log.Acao,
                    NomeTabela = log.NomeTabela,
                    ChavePrimariaRegistro = log.ChavePrimariaRegistro,
                    DescricaoRegistro = descricao,
                    InventarioId = inventarioId // Adiciona o ID do inventário encontrado
                });
            }

            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Criar()
        {
            ViewBag.Funcoes = new SelectList(_roleManager.Roles, "Name", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(CriarUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, NomeCompleto = model.NomeCompleto,MudarSenhaPrimeiroAcesso = true };
                var result = await _userManager.CreateAsync(user, model.Senha);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, model.FuncaoId);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in result.Errors) { ModelState.AddModelError(string.Empty, error.Description); }
            }
            ViewBag.Funcoes = new SelectList(_roleManager.Roles, "Name", "Name", model.FuncaoId);
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Editar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();

            var funcoesUsuario = await _userManager.GetRolesAsync(usuario);
            var viewModel = new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                NomeCompleto = usuario.NomeCompleto,
                Email = usuario.Email,
                Funcao = funcoesUsuario.FirstOrDefault(),
                TodasAsFuncoes = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList())
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(EditarUsuarioViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await _userManager.FindByIdAsync(model.Id);
                if (usuario == null) return NotFound();

                usuario.NomeCompleto = model.NomeCompleto;
                usuario.Email = model.Email;
                usuario.UserName = model.Email;

                var updateResult = await _userManager.UpdateAsync(usuario);

                if (updateResult.Succeeded)
                {
                    var funcoesAtuais = await _userManager.GetRolesAsync(usuario);
                    await _userManager.RemoveFromRolesAsync(usuario, funcoesAtuais);
                    await _userManager.AddToRoleAsync(usuario, model.Funcao);
                    return RedirectToAction(nameof(Index));
                }
                foreach (var error in updateResult.Errors) { ModelState.AddModelError(string.Empty, error.Description); }
            }
            model.TodasAsFuncoes = new SelectList(_roleManager.Roles.Select(r => r.Name).ToList());
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Deletar(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario == null) return NotFound();
            return View(usuario);
        }

        [HttpPost, ActionName("Deletar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletarConfirmado(string id)
        {
            var usuario = await _userManager.FindByIdAsync(id);
            if (usuario != null)
            {
                await _userManager.DeleteAsync(usuario);
            }
            return RedirectToAction(nameof(Index));
        }
    }
}