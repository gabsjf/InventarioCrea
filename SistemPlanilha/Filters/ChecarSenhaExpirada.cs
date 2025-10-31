using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SistemPlanilha.Models;
using System;
using System.Threading.Tasks;

public class ChecarSenhaExpirada : IAsyncActionFilter
{
    private readonly UserManager<ApplicationUser> _userManager;
    private const int PasswordExpiryDays = 90;

    public ChecarSenhaExpirada(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (context.HttpContext.User.Identity.IsAuthenticated)
        {
            var user = await _userManager.GetUserAsync(context.HttpContext.User);
            var controllerName = context.RouteData.Values["controller"]?.ToString();
            var actionName = context.RouteData.Values["action"]?.ToString();

            bool isOnChangePasswordPage = (controllerName == "Conta" && (actionName == "ChangePassword" || actionName == "ChangePasswordOnFirstLogin" || actionName == "Sair"));

            if (user != null && !isOnChangePasswordPage)
            {
                bool needsChange = user.PasswordLastChangedDate == null ||
                                   user.PasswordLastChangedDate.Value.AddDays(PasswordExpiryDays) < DateTime.UtcNow;

                if (needsChange && !user.MudarSenhaPrimeiroAcesso)
                {
                    context.Result = new RedirectToActionResult("ChangePassword", "Conta", new { reason = "expired" });
                    return;
                }
            }
        }
        await next();
    }
}