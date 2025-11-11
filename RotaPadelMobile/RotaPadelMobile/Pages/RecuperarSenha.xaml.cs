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
                    await DisplayAlert("Erro", "Erro ao enviar email. Não ira ser possivel enviar e-mail por conta de restriçoes do android, referente a sistemas SMTP", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro: {ex.Message}", "OK");
            }
        }
    }
}