using SistemPlanilha.ViewModels.Usuario;

public class UsuarioDetalhesViewModel
{
    public string NomeCompleto { get; set; }
    public string Email { get; set; }
    public List<AuditLogViewModel> LogsDeAtividade { get; set; }

    public UsuarioDetalhesViewModel()
    {
        LogsDeAtividade = new List<AuditLogViewModel>();
    }
}