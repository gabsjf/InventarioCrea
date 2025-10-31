// Em Validators/PreviousPasswordValidator.cs
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SistemPlanilha.Data; // Seu namespace do BancoContext
using SistemPlanilha.Models; // Seu namespace do ApplicationUser
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class PreviousPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : ApplicationUser
{
    private readonly BancoContext _context;
    private const int MaxHistoryCheck = 5; // Impedir reutilização das últimas 5 senhas

    public PreviousPasswordValidator(BancoContext context)
    {
        _context = context;
    }

    public async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
    {
        // Pega os hashes das senhas anteriores do usuário
        var recentHashes = await _context.PasswordHistories
            .Where(ph => ph.UserId == user.Id)
            .OrderByDescending(ph => ph.DateCreated)
            .Take(MaxHistoryCheck)
            .Select(ph => ph.PasswordHash)
            .ToListAsync();

        if (!recentHashes.Any())
        {
            return IdentityResult.Success; // Sem histórico, permite qualquer senha
        }

        // Verifica se o hash da *nova* senha corresponde a algum dos hashes recentes
        // IMPORTANTE: O PasswordHasher lida com os salts automaticamente
        var passwordHasher = new PasswordHasher<TUser>();
        foreach (var oldHash in recentHashes)
        {
            var verificationResult = passwordHasher.VerifyHashedPassword(user, oldHash, password);
            if (verificationResult == PasswordVerificationResult.Success || verificationResult == PasswordVerificationResult.SuccessRehashNeeded)
            {
                // A senha nova é igual a uma das antigas!
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "PasswordReuse",
                    Description = $"Você não pode reutilizar uma das suas últimas {MaxHistoryCheck} senhas."
                });
            }
        }

        // A senha nova é diferente de todas as recentes
        return IdentityResult.Success;
    }
}