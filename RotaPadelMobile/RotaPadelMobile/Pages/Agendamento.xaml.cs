using RotaPadelMobile.Models;
using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class Agendamento : ContentPage
    {
        private readonly DatabaseService _database;
        private readonly int _usuarioId;
        private readonly List<Button> botoesSelecionados = new();

        public Agendamento(int usuarioId)
        {
            InitializeComponent();
            _usuarioId = usuarioId;
            _database = new DatabaseService();

            // Atualiza disponibilidade ao mudar a data ou a quadra
            pickerData.DateSelected += pickerData_DateSelected;
            pickerQuadra.SelectedIndexChanged += pickerQuadra_SelectedIndexChanged;

            AtualizarDisponibilidade();
        }

        // Atualiza cores dos horários conforme disponibilidade
        private async void AtualizarDisponibilidade()
        {
            if (pickerQuadra.SelectedItem == null)
                return;

            var quadra = pickerQuadra.SelectedItem.ToString();
            var data = pickerData.Date.Date;

            var agendamentos = await _database.ObterAgendamentosPorDataEQuadra(data, quadra);

            // Pega todos os botões de horário das grids
            var botoes = gridManhã.Children
                .Concat(gridTarde.Children)
                .Concat(gridNoite.Children)
                .OfType<Button>()
                .Where(b => b.Text.Contains(":"));

            foreach (var botao in botoes)
            {
                botao.IsEnabled = true;
                botao.TextColor = Colors.Black;

                // Cinza claro = disponível
                botao.BackgroundColor = Colors.LightGray;

                // Se horário estiver ocupado
                if (agendamentos.Any(a => a.Hora == botao.Text))
                {
                    botao.BackgroundColor = Colors.DarkGray;
                    botao.IsEnabled = false;
                }
                // Se for um horário passado no dia atual
                else if (data == DateTime.Today && TimeSpan.Parse(botao.Text) < DateTime.Now.TimeOfDay)
                {
                    botao.BackgroundColor = Colors.LightPink;
                    botao.IsEnabled = false;
                }
            }

            // Limpa seleções anteriores
            botoesSelecionados.Clear();
        }

        // Clicar em um horário (selecionar/deselecionar)
        private void bt_Clicked(object sender, EventArgs e)
        {
            var botaoClicado = (Button)sender;

            // Se já está selecionado → desmarca
            if (botoesSelecionados.Contains(botaoClicado))
            {
                botoesSelecionados.Remove(botaoClicado);
                botaoClicado.BackgroundColor = Colors.LightGray;
                botaoClicado.TextColor = Colors.Black;
            }
            else // Seleciona
            {
                botoesSelecionados.Add(botaoClicado);
                botaoClicado.BackgroundColor = Color.FromArgb("#90EE90"); // Verde claro
                botaoClicado.TextColor = Colors.Black;
            }
        }

        // Confirmar reserva
        private async void btConfirmar_Clicked(object sender, EventArgs e)
        {
            if (pickerQuadra.SelectedItem == null)
            {
                await DisplayAlert("Aviso", "Selecione uma quadra.", "OK");
                return;
            }

            if (botoesSelecionados.Count == 0)
            {
                await DisplayAlert("Aviso", "Selecione pelo menos um horário antes de confirmar.", "OK");
                return;
            }

            var quadra = pickerQuadra.SelectedItem.ToString();
            var data = pickerData.Date.Date;

            int reservasCriadas = 0;

            foreach (var botao in botoesSelecionados)
            {
                // Garante que o horário ainda está disponível
                if (await _database.HorarioOcupado(quadra, data, botao.Text))
                    continue;

                var novaReserva = new Reserva
                {
                    Quadra = quadra,
                    Data = data,
                    Hora = botao.Text,
                    UsuarioId = _usuarioId
                };

                await _database.CadastrarReserva(novaReserva);
                reservasCriadas++;
            }

            await DisplayAlert("Sucesso", $"{reservasCriadas} horário(s) reservado(s) com sucesso!", "OK");

            AtualizarDisponibilidade();
        }

        private void pickerData_DateSelected(object sender, DateChangedEventArgs e)
        {
            AtualizarDisponibilidade();
        }

        private void pickerQuadra_SelectedIndexChanged(object sender, EventArgs e)
        {
            AtualizarDisponibilidade();
        }
    }
}
