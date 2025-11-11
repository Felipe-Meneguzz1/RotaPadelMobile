using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class Login : ContentPage
    {
        private readonly DatabaseService _database;

        public Login()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _database = new DatabaseService();
        }

        private async void OnEntrarClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha o email", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntrySenha.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha a senha", "OK");
                return;
            }

            try
            {
                var senhaHash = PasswordHelper.HashPassword(EntrySenha.Text);
                var usuario = await _database.Login(EntryEmail.Text.Trim().ToLower(), senhaHash);

                if (usuario != null)
                {
                    // ? Passa o usuário logado para a próxima tela
                    await Navigation.PushAsync(new Agendamento(usuario.Id));
                }
                else
                {
                    await DisplayAlert("Erro", "Email ou senha incorretos", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao fazer login: {ex.Message}", "OK");
            }
        }

        private async void OnEsqueciSenhaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RecuperarSenha());
        }

        private async void OnCadastroClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cadastro());
        }
    }
}
