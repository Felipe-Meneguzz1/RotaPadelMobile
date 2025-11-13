using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class AlterarSenha : ContentPage
    {
        private readonly DatabaseService _database;

        public AlterarSenha()
        {
            InitializeComponent();
            _database = new DatabaseService();
        }

        private async void OnAlterarSenhaClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntrySenhaAtual.Text))
            {
                await DisplayAlert("Erro", "Digite sua senha atual", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntryNovaSenha.Text))
            {
                await DisplayAlert("Erro", "Digite a nova senha", "OK");
                return;
            }

            if (EntryNovaSenha.Text.Length < 6)
            {
                await DisplayAlert("Erro", "A senha deve ter no mínimo 6 caracteres", "OK");
                return;
            }

            if (EntryNovaSenha.Text != EntryConfirmarSenha.Text)
            {
                await DisplayAlert("Erro", "As senhas não coincidem", "OK");
                return;
            }

            if (EntrySenhaAtual.Text == EntryNovaSenha.Text)
            {
                await DisplayAlert("Erro", "A nova senha deve ser diferente da atual", "OK");
                return;
            }

            try
            {
                var senhaAtualHash = PasswordHelper.HashPassword(EntrySenhaAtual.Text);
                var usuario = await _database.Login(UsuarioLogado.Email, senhaAtualHash);

                if (usuario == null)
                {
                    await DisplayAlert("Erro", "Senha atual incorreta", "OK");
                    return;
                }
                var novaSenhaHash = PasswordHelper.HashPassword(EntryNovaSenha.Text);
                await _database.AtualizarSenha(usuario.Id, novaSenhaHash);

                await DisplayAlert("Sucesso", "Senha alterada com sucesso!", "OK");
                EntrySenhaAtual.Text = string.Empty;
                EntryNovaSenha.Text = string.Empty;
                EntryConfirmarSenha.Text = string.Empty;

                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao alterar senha: {ex.Message}", "OK");
            }
        }
    }
}