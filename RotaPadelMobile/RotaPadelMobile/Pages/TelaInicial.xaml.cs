namespace RotaPadelMobile.Pages;

public partial class TelaInicial : ContentPage
{
	public TelaInicial()
	{
		InitializeComponent();
	}

    private async void Button_Login(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Login());
    }

    private async void Button_Cadastro(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new Cadastro());
    }
}