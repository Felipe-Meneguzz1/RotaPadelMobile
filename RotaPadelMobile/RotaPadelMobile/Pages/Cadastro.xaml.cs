using RotaPadelMobile.Models;
using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class Cadastro : ContentPage
    {
        private readonly DatabaseService _database;

        public Cadastro()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _database = new DatabaseService();
        }

        private async void OnCadastrarClicked(object sender, EventArgs e)
        {
            // Validações
            if (string.IsNullOrWhiteSpace(EntryNome.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha o nome", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha o email", "OK");
                return;
            }

            if (!IsValidEmail(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Por favor, insira um email válido", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntryCPF.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha o CPF", "OK");
                return;
            }

            if (EntryCPF.Text.Length != 11)
            {
                await DisplayAlert("Erro", "CPF deve conter 11 dígitos", "OK");
                return;
            }

            if (string.IsNullOrWhiteSpace(EntrySenha.Text))
            {
                await DisplayAlert("Erro", "Por favor, preencha a senha", "OK");
                return;
            }

            if (EntrySenha.Text.Length < 6)
            {
                await DisplayAlert("Erro", "A senha deve ter no mínimo 6 caracteres", "OK");
                return;
            }

            if (EntrySenha.Text != EntryConfirmarSenha.Text)
            {
                await DisplayAlert("Erro", "As senhas não coincidem", "OK");
                return;
            }

            // Verificar se o email já existe
            if (await _database.EmailExiste(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Este email já está cadastrado", "OK");
                return;
            }

            try
            {
                // Criar o usuário
                var usuario = new Usuario
                {
                    Nome = EntryNome.Text.Trim(),
                    Email = EntryEmail.Text.Trim().ToLower(),
                    CPF = EntryCPF.Text.Trim(),
                    Senha = PasswordHelper.HashPassword(EntrySenha.Text),
                    Perfil = "User",
                    DataCriacao = DateTime.Now
                };

                // Salvar no banco
                await _database.CadastrarUsuario(usuario);

                await DisplayAlert("Sucesso", "Cadastro realizado com sucesso!", "OK");

                // Limpar os campos
                LimparCampos();

                // Voltar para a tela anterior
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao cadastrar: {ex.Message}", "OK");
            }
        }

        private void OnLoginClicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void LimparCampos()
        {
            EntryNome.Text = string.Empty;
            EntryEmail.Text = string.Empty;
            EntryCPF.Text = string.Empty;
            EntrySenha.Text = string.Empty;
            EntryConfirmarSenha.Text = string.Empty;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}