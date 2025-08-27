using Microsoft.Extensions.Logging;
using CitasMedicasApp.Services;
using CitasMedicasApp.ViewModels;
using CitasMedicasApp.Views;

namespace CitasMedicasApp;

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
                fonts.AddFont("Segoe-UI.ttf", "SegoeUI");
            });

        // Registrar servicios
        builder.Services.AddSingleton<IApiService, ApiService>();
        builder.Services.AddSingleton<IAuthService, AuthService>();

        // Registrar ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        builder.Services.AddTransient<MainMenuViewModel>();
        builder.Services.AddTransient<CambiarPasswordViewModel>();
        builder.Services.AddTransient<HistorialViewModel>();
        builder.Services.AddTransient<CitasViewModel>();

        // Registrar Views
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<MainMenuPage>();
        builder.Services.AddTransient<CambiarPasswordPage>();
        builder.Services.AddTransient<HistorialPage>();
        builder.Services.AddTransient<CitasPage>();
        builder.Services.AddTransient<ConfigPage>();
        // ConsultarCitasPage es opcional por ahora
        builder.Services.AddTransient<ConsultarCitasPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}