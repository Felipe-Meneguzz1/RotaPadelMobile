namespace RotaPadelMobile.Pages;

public partial class Agendamento : ContentPage
{
    
    public Agendamento()
	{
		InitializeComponent();
	}

    // Lista que guarda todos os botões atualmente selecionados
    private readonly List<Button> botoesSelecionados = new();

    private void bt_Clicked(object sender, EventArgs e)
    {
        var botaoClicado = (Button)sender;

        // Se o botão já estava selecionado → desmarca
        if (botoesSelecionados.Contains(botaoClicado))
        {
            botoesSelecionados.Remove(botaoClicado);
            botaoClicado.BackgroundColor = Colors.LightGray;
            botaoClicado.TextColor = Colors.Black;
        }
        else // caso contrário, marca
        {
            botoesSelecionados.Add(botaoClicado);
            botaoClicado.BackgroundColor = Color.FromArgb("#90EE90"); // Verde claro
            botaoClicado.TextColor = Colors.Black;
        }
    }

    private void btConfirmar_Clicked(object sender, EventArgs e)
    {
        if (botoesSelecionados.Count == 0)
        {
            DisplayAlert("Aviso", "Selecione pelo menos um horário antes de confirmar.", "OK");
            return;
        }

    }
}