using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.Models; 
using SistemPlanilha.Data;  
using SistemPlanilha.ViewModels.Conta;
using System.Threading.Tasks;
using System; 
namespace SistemPlanilha.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly BancoContext _context;
        private const int DiasExpiracaoSenha = 90;

        public ContaController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            BancoContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context; 
        }

        [HttpGet]
        public IActionResult Registrar()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registrar(RegistrarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    NomeCompleto = model.NomeCompleto,
                    MudarSenhaPrimeiroAcesso = true
                };

                var result = await _userManager.CreateAsync(user, model.Senha);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Tecnico");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("ChangePasswordOnFirstLogin");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, model.LembrarMe, lockoutOnFailure: false);

                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Email);

                    if (user != null && user.MudarSenhaPrimeiroAcesso)
                    {
                        return RedirectToAction("ChangePasswordOnFirstLogin");
                    }

                    if (user != null && (user.PasswordLastChangedDate == null || user.PasswordLastChangedDate.Value.AddDays(DiasExpiracaoSenha) < DateTime.UtcNow))
                    {
                        return RedirectToAction("ChangePassword", new { reason = "expired" });
                    }

                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "Email ou senha inválidos.");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sair()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Authorize]
        public IActionResult ChangePasswordOnFirstLogin()
        {
            return View();
        }


        [HttpGet]
        [Authorize]
        public IActionResult ChangePassword(string reason = null)
        {
            if (reason == "expired")
            {
                TempData["StatusMessage"] = "Sua senha expirou. Por favor, defina uma nova senha para continuar.";
            }
            return View();
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }

            var oldHash = user.PasswordHash; 

            var changePasswordResult = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!changePasswordResult.Succeeded)
            {
               
                foreach (var error in changePasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            
            if (!string.IsNullOrEmpty(oldHash))
            {
                var historyEntry = new PasswordHistory
                {
                    UserId = user.Id,
                    PasswordHash = oldHash,
                    DateCreated = user.PasswordLastChangedDate ?? DateTime.UtcNow
                };
                _context.PasswordHistories.Add(historyEntry);
            }
           

            user.PasswordLastChangedDate = DateTime.UtcNow;

            await _userManager.UpdateAsync(user); 
            await _context.SaveChangesAsync(); 

            await _signInManager.RefreshSignInAsync(user);
            TempData["StatusMessage"] = "Sua senha foi alterada com sucesso.";
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePasswordOnFirstLogin(MudarSenhaPrimeiroAcessoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null) { return NotFound(); }

            var tempHash = user.PasswordHash;

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded) { throw new InvalidOperationException($"Erro ao remover senha temporária do usuário '{user.Id}'."); }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
               
                foreach (var error in addPasswordResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                return View(model);
            }

            
            if (!string.IsNullOrEmpty(tempHash))
            {
                var historyEntry = new PasswordHistory
                {
                    UserId = user.Id,
                    PasswordHash = tempHash,
                    DateCreated = DateTime.UtcNow
                };
                _context.PasswordHistories.Add(historyEntry);
            }
          

            user.MudarSenhaPrimeiroAcesso = false;
            user.PasswordLastChangedDate = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);
            await _context.SaveChangesAsync(); 

            await _signInManager.RefreshSignInAsync(user);
            return RedirectToAction("Index", "Home");
        }
    }
}