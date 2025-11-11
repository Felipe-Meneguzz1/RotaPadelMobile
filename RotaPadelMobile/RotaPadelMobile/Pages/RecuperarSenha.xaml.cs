using RotaPadelMobile.Models;
using RotaPadelMobile.Services;
using System.Runtime.Intrinsics.X86;

namespace RotaPadelMobile.Pages
{
    public partial class RecuperarSenha : ContentPage
    {
        private readonly DatabaseService _database;
        private string _emailRecuperacao;
        private int _codigoId;

        public RecuperarSenha()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            _database = new DatabaseService();
        }

        private async void OnEnviarCodigoClicked(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(EntryEmail.Text))
            {
                await DisplayAlert("Erro", "Erro ao enviar email. Não ira ser possivel enviar e-mail por conta de restriçoes do android, referente a sistemas SMTP", "OK");
                return;
            }

            // Verificar se o email existe
            var usuario = await _database.ObterUsuarioPorEmail(EntryEmail.Text.Trim().ToLower());
            if (usuario == null)
            {
                await DisplayAlert("Erro", "Erro ao enviar email. Não ira ser possivel enviar e-mail por conta de restriçoes do android, referente a sistemas SMTP", "OK");
                return;
            }
        }
    }
            
}