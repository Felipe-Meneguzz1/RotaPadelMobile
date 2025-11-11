namespace RotaPadelMobile
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            var navigationPage = new NavigationPage(new Pages.Reservas())
            {
                BarBackgroundColor = Colors.White,
                BarTextColor = Colors.Black
            };

            return new Window(navigationPage);
        }
    }
}