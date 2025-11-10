using RotaPadelMobile.Models;
using RotaPadelMobile.Services;
using System.Runtime.Intrinsics.X86;

namespace RotaPadelMobile.Pages
{
    public partial class RecuperarSenha : ContentPage
    {
        private readonly DatabaseService _database;
        private readonly EmailService _emailService;
        private string _emailRecuperacao;
        private int _codigoId;

        public RecuperarSenha()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _database = new DatabaseService();
            _emailService = new EmailService();
        }

        private async void OnEnviarCodigoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Digite seu email", "OK");
                return;
            }

            // Verificar se o email existe
            var usuario = await _database.ObterUsuarioPorEmail(EntryEmail.Text.Trim().ToLower());
            if (usuario == null)
            {
                await DisplayAlert("Erro", "Email não encontrado", "OK");
                return;
            }

            try
            {
                // Gerar código
                var codigo = _emailService.GerarCodigoRecuperacao();

                // Salvar no banco
                var codigoRecuperacao = new CodigoRecuperacao
                {
                    Email = EntryEmail.Text.Trim().ToLower(),
                    Codigo = codigo,
                    DataCriacao = DateTime.Now,
                    DataExpiracao = DateTime.Now.AddMinutes(15),
                    Usado = false
                };
                await _database.SalvarCodigoRecuperacao(codigoRecuperacao);

                // Enviar email
                var enviado = await _emailService.EnviarCodigoRecuperacao(EntryEmail.Text, codigo);

                if (enviado)
                {
                    _emailRecuperacao = EntryEmail.Text.Trim().ToLower();
                    Step1.IsVisible = false;
                    Step2.IsVisible = true;
                    await DisplayAlert("Sucesso", "Código enviado para seu email!", "OK");
                }
                else
                {
                    await DisplayAlert("Erro", "Erro ao enviar email. Tente novamente.", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro: {ex.Message}", "OK");
            }
        }

        private async void OnValidarCodigoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryCodigo.Text))
            {
                await DisplayAlert("Erro", "Digite o código", "OK");
                return;
            }

            var codigoValido = await _database.ValidarCodigo(_emailRecuperacao, EntryCodigo.Text);

            if (codigoValido != null)
            {
                _codigoId = codigoValido.Id;
                Step2.IsVisible = false;
                Step3.IsVisible = true;
            }
            else
            {
                await DisplayAlert("Erro", "Código inválido ou expirado", "OK");
            }
        }

        private async void OnAlterarSenhaClicked(object sender, EventArgs e)
        {
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

            if (EntryNovaSenha.Text != EntryConfirmarNovaSenha.Text)
            {
                await DisplayAlert("Erro", "As senhas não coincidem", "OK");
                return;
            }

            try
            {
                var usuario = await _database.ObterUsuarioPorEmail(_emailRecuperacao);
                var senhaHash = PasswordHelper.HashPassword(EntryNovaSenha.Text);

                await _database.AtualizarSenha(usuario.Id, senhaHash);
                await _database.MarcarCodigoComoUsado(_codigoId);

                await DisplayAlert("Sucesso", "Senha alterada com sucesso!", "OK");
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao alterar senha: {ex.Message}", "OK");
            }
        }
    }
}