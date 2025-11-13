using RotaPadelMobile.Models;
using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class EditarPerfil : ContentPage
    {
        private readonly DatabaseService _database;
        private Usuario _usuario;

        public EditarPerfil()
        {
            InitializeComponent();
            _database = new DatabaseService();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarDadosUsuario();
        }

        private async Task CarregarDadosUsuario()
        {
            try
            {
                _usuario = await _database.ObterUsuarioPorId(UsuarioLogado.Id);

                if (_usuario != null)
                {
                    EntryNome.Text = _usuario.Nome;
                    EntryEmail.Text = _usuario.Email;
                    EntryCPF.Text = _usuario.CPF;
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar dados: {ex.Message}", "OK");
            }
        }

        private async void OnSalvarClicked(object sender, EventArgs e)
        {
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
            if (await _database.EmailJaExiste(EntryEmail.Text.Trim().ToLower(), UsuarioLogado.Id))
            {
                await DisplayAlert("Erro", "Este email já está cadastrado para outro usuário", "OK");
                return;
            }

            try
            {
                _usuario.Nome = EntryNome.Text.Trim();
                _usuario.Email = EntryEmail.Text.Trim().ToLower();

                await _database.AtualizarUsuario(_usuario);

                UsuarioLogado.Nome = _usuario.Nome;
                UsuarioLogado.Email = _usuario.Email;

                await DisplayAlert("Sucesso", "Perfil atualizado com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao salvar: {ex.Message}", "OK");
            }
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