using SistemPlanilha.Data;
using SistemPlanilha.Models;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace SistemPlanilha.Application
{
    public class AuditService : IAuditService
    {
        private readonly BancoContext _context;

        public AuditService(BancoContext context)
        {
            _context = context;
        }

        public async Task RegistrarAuditoria(TipoAcao acao, string nomeTabela, int chavePrimaria, string usuario, object? dadosAlterados = null)
        {
            var log = new AuditLog
            {
                Acao = acao,
                NomeTabela = nomeTabela,
                ChavePrimariaRegistro = chavePrimaria,
                Usuario = usuario,
                Timestamp = DateTime.UtcNow,
                Alteracoes = dadosAlterados != null ? JsonSerializer.Serialize(dadosAlterados) : null
            };

            await _context.Auditoria.AddAsync(log);
            await _context.SaveChangesAsync();
        }
    }
}