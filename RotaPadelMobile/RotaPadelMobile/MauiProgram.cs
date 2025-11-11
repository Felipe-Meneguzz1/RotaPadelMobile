using Microsoft.Extensions.Logging;
using RotaPadelMobile.Services;

namespace RotaPadelMobile
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("InstrumentSans-Regular.ttf", "InstrumentSans");
                    fonts.AddFont("InstrumentSans-Italic.ttf", "InstrumentSansItalic");
                });

            //DatabaseService
            builder.Services.AddSingleton<DatabaseService>();


#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
