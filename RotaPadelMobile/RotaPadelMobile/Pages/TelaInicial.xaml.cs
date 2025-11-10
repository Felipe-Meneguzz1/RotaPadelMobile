namespace RotaPadelMobile.Pages
{
    public partial class TelaInicial : ContentPage
    {
        public TelaInicial()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
        }

        private async void Button_Login(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Login());
        }

        private async void Button_Cadastro(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Cadastro());
        }
    }
}