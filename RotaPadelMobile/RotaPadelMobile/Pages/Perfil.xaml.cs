using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class Perfil : ContentPage
    {
        public Perfil()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            CarregarDadosUsuario();
        }

        private void CarregarDadosUsuario()
        {
            if (UsuarioLogado.EstaLogado)
            {
                LabelNome.Text = UsuarioLogado.Nome;
                LabelEmail.Text = UsuarioLogado.Email;
            }
        }

        private async void OnEditarPerfilClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new EditarPerfil());
        }

        private async void OnAlterarSenhaClicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AlterarSenha());
        }

        private async void OnSairClicked(object sender, EventArgs e)
        {
            bool confirmar = await DisplayAlert(
                "Sair",
                "Tem certeza que deseja sair?",
                "Sim",
                "Não"
            );

            if (confirmar)
            {
                UsuarioLogado.Limpar();
                Application.Current.MainPage = new NavigationPage(new TelaInicial());
            }
        }
    }
}