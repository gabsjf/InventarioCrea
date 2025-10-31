using Microsoft.AspNetCore.Identity;
using SistemPlanilha.Models;
using System;
using System.Threading.Tasks;

namespace SistemPlanilha.Data
{
    public static class InicializadorDeDados
    {
      
        public static async Task SeedData(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            await SeedRoles(roleManager);
            await SeedAdminUser(userManager);
        }

        private static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
        {
            string[] funcoes = { "Administrador", "Tecnico" };

            foreach (var funcao in funcoes)
            {
                if (!await roleManager.RoleExistsAsync(funcao))
                {
                    await roleManager.CreateAsync(new IdentityRole(funcao));
                }
            }
        }

        private static async Task SeedAdminUser(UserManager<ApplicationUser> userManager)
        {
           
            if (await userManager.FindByEmailAsync("admin@admin.com") == null)
            {
                var adminUser = new ApplicationUser
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    NomeCompleto = "Administrador do Sistema",
                    EmailConfirmed = true 
                };

                
                IdentityResult result = await userManager.CreateAsync(adminUser, "Senha@123");

                if (result.Succeeded)
                {
                    
                    await userManager.AddToRoleAsync(adminUser, "Administrador");
                }
            }
        }
    }
}