using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SistemPlanilha.ViewModels.Conta;
using System.Threading.Tasks;

namespace SistemPlanilha.Controllers
{
    public class ContaController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;

        public ContaController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
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
               
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                
                var result = await _userManager.CreateAsync(user, model.Senha);

                if (result.Succeeded)
                {
                    
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
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
    }
}