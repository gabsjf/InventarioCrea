namespace SistemPlanilha.ViewModels.Usuario
{
    public class UsuarioListagemViewModel
    {
        public string Id { get; set; }
        public string NomeCompleto { get; set; }
        public string Email { get; set; }
        public string Funcao { get; set; } // Role (Administrador ou Tecnico)
    }
}