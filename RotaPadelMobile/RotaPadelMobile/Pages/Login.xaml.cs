namespace RotaPadelMobile.Pages
{
    public partial class Login : ContentPage
    {
        public Login()
        {
            InitializeComponent();
        }

        private async void OnEntrarClicked(object sender, EventArgs e)
        {

            await DisplayAlert("Login", "Funcionalidade em desenvolvimento", "OK");
        }

        private async void OnEsqueciSenhaClicked(object sender, EventArgs e)
        {
            await DisplayAlert("Esqueci minha senha", "Funcionalidade em desenvolvimento", "OK");
        }

        private async void OnCadastroClicked(object sender, EventArgs e)
        {
            // Volta para tela inicial e abre cadastro
            await Navigation.PopModalAsync();
            await Navigation.PushModalAsync(new Cadastro());
        }
    }
}