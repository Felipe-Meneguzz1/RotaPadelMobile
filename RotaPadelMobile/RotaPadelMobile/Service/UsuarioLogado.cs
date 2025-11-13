namespace RotaPadelMobile.Services
{
    public static class UsuarioLogado
    {
        public static int Id { get; set; }
        public static string Nome { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static string Perfil { get; set; } = "User";

        public static bool EstaLogado => Id > 0;

        public static void Limpar()
        {
            Id = 0;
            Nome = string.Empty;
            Email = string.Empty;
            Perfil = string.Empty;
        }
    }
}