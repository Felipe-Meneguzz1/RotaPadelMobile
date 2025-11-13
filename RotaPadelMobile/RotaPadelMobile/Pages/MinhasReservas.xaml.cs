using Microsoft.Maui.Controls.Shapes;
using RotaPadelMobile.Models;
using RotaPadelMobile.Services;

namespace RotaPadelMobile.Pages
{
    public partial class MinhasReservas : ContentPage
    {
        private readonly DatabaseService _database;
        private readonly int _usuarioId;
        public MinhasReservas()
        {
            InitializeComponent();
            _database = new DatabaseService();
            _usuarioId = UsuarioLogado.Id; 
        }
        public MinhasReservas(int usuarioId)
        {
            InitializeComponent();
            _database = new DatabaseService();
            _usuarioId = usuarioId;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await CarregarReservas();
        }

        private async Task CarregarReservas()
        {
            try
            {
                StackReservas.Children.Clear();

                var reservas = await _database.ObterReservasPorUsuario(_usuarioId);

                if (reservas == null || reservas.Count == 0)
                {
                    BorderSemReservas.IsVisible = true;
                    StackReservas.IsVisible = false;
                    return;
                }

                BorderSemReservas.IsVisible = false;
                StackReservas.IsVisible = true;

                var reservasFuturas = reservas.Where(r => !_database.JaPassou(r)).ToList();
                var reservasPassadas = reservas.Where(r => _database.JaPassou(r)).ToList();

                if (reservasFuturas.Any())
                {
                    var labelFuturas = new Label
                    {
                        Text = "Próximas Reservas",
                        FontSize = 22,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.Black,
                        FontFamily = "InstrumentSans",
                        Margin = new Thickness(0, 0, 0, 10)
                    };
                    StackReservas.Children.Add(labelFuturas);

                    foreach (var reserva in reservasFuturas)
                    {
                        var card = CriarCardReserva(reserva);
                        StackReservas.Children.Add(card);
                    }
                }
                if (reservasPassadas.Any())
                {
                    var labelPassadas = new Label
                    {
                        Text = "Histórico",
                        FontSize = 22,
                        FontAttributes = FontAttributes.Bold,
                        TextColor = Colors.Black,
                        FontFamily = "InstrumentSans",
                        Margin = new Thickness(0, 20, 0, 10)
                    };
                    StackReservas.Children.Add(labelPassadas);

                    foreach (var reserva in reservasPassadas)
                    {
                        var card = CriarCardReserva(reserva);
                        StackReservas.Children.Add(card);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", $"Erro ao carregar reservas: {ex.Message}", "OK");
            }
        }

        private Border CriarCardReserva(Reserva reserva)
        {
            var jaPassou = _database.JaPassou(reserva);
            var podeCancelar = _database.PodeCancelar(reserva);

            var card = new Border
            {
                BackgroundColor = Colors.White,
                Padding = new Thickness(20),
                Margin = new Thickness(0, 0, 0, 0),
                StrokeThickness = 0
            };

            card.StrokeShape = new RoundRectangle { CornerRadius = 20 };

            var stack = new VerticalStackLayout { Spacing = 8 };
            stack.Children.Add(new Label
            {
                Text = reserva.Quadra,
                FontSize = 22,
                FontAttributes = FontAttributes.Bold,
                TextColor = Colors.Black,
                FontFamily = "InstrumentSans"
            });
            stack.Children.Add(new Label
            {
                Text = $"Data: {reserva.Data:dd/MM/yyyy}",
                FontSize = 15,
                TextColor = Colors.Black,
                FontFamily = "InstrumentSans"
            });
            stack.Children.Add(new Label
            {
                Text = $"Horário: {reserva.Hora}",
                FontSize = 15,
                TextColor = Colors.Black,
                FontFamily = "InstrumentSans"
            });
            if (jaPassou)
            {
                stack.Children.Add(new Label
                {
                    Text = "Já passou",
                    FontSize = 13,
                    TextColor = Color.FromArgb("#666"),
                    FontAttributes = FontAttributes.Italic,
                    FontFamily = "InstrumentSans",
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }
            else
            {
                var horaReserva = TimeSpan.Parse(reserva.Hora);
                var dataHoraReserva = reserva.Data.Date + horaReserva;
                var tempoRestante = dataHoraReserva - DateTime.Now;

                string textoTempo;
                if (tempoRestante.TotalDays >= 1)
                {
                    int dias = (int)tempoRestante.TotalDays;
                    textoTempo = $"Faltam {dias} dia{(dias > 1 ? "s" : "")}";
                }
                else if (tempoRestante.TotalHours >= 1)
                {
                    int horas = (int)tempoRestante.TotalHours;
                    int minutos = tempoRestante.Minutes;
                    textoTempo = $"Faltam {horas}h {minutos}min";
                }
                else
                {
                    textoTempo = $"Faltam {tempoRestante.Minutes}min";
                }

                stack.Children.Add(new Label
                {
                    Text = textoTempo,
                    FontSize = 13,
                    TextColor = Color.FromArgb("#00C853"),
                    FontAttributes = FontAttributes.Bold,
                    FontFamily = "InstrumentSans",
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }
            if (!podeCancelar && !jaPassou)
            {
                stack.Children.Add(new Label
                {
                    Text = "Não pode cancelar: faltam menos de 2h",
                    FontSize = 12,
                    TextColor = Color.FromArgb("#FF6B6B"),
                    FontAttributes = FontAttributes.Italic,
                    FontFamily = "InstrumentSans",
                    Margin = new Thickness(0, 5, 0, 0)
                });
            }
            if (podeCancelar)
            {
                var btnCancelar = new Button
                {
                    Text = "Cancelar",
                    BackgroundColor = Color.FromArgb("#FF6B6B"),
                    TextColor = Colors.White,
                    FontFamily = "InstrumentSans",
                    FontAttributes = FontAttributes.Bold,
                    CornerRadius = 20,
                    HeightRequest = 45,
                    Margin = new Thickness(0, 15, 0, 0)
                };

                btnCancelar.Clicked += async (s, e) => await CancelarReserva(reserva);
                stack.Children.Add(btnCancelar);
            }

            card.Content = stack;
            return card;
        }

        private async Task CancelarReserva(Reserva reserva)
        {
            bool confirmar = await DisplayAlert(
                "Cancelar Reserva",
                $"Tem certeza que deseja cancelar a reserva de {reserva.Quadra} para {reserva.Data:dd/MM/yyyy} às {reserva.Hora}?",
                "Sim, cancelar",
                "Não"
            );

            if (confirmar)
            {
                try
                {
                    await _database.CancelarReserva(reserva.Id);
                    await DisplayAlert("Sucesso", "Reserva cancelada com sucesso!", "OK");
                    await CarregarReservas();
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Erro", $"Erro ao cancelar: {ex.Message}", "OK");
                }
            }
        }
    }
}