using CitasMedicasApp.Services;

namespace CitasMedicasApp.Views;

public partial class ConfigPage : ContentPage
{
    private readonly IAuthService? _authService;

    public ConfigPage()
    {
        InitializeComponent();
    }

    // Constructor con inyección de dependencias
    public ConfigPage(IAuthService authService) : this()
    {
        _authService = authService;
    }

    private async void OnCambiarPasswordClicked(object sender, EventArgs e)
    {
        try
        {
            await Shell.Current.GoToAsync("cambiarpassword");
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudo navegar: {ex.Message}", "OK");
        }
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        bool confirm = await DisplayAlert("Cerrar Sesión",
            "¿Está seguro que desea cerrar sesión?", "Sí", "No");

        if (confirm)
        {
            try
            {
                // Lógica de logout
                if (_authService != null)
                {
                    await _authService.LogoutAsync();
                }

                if (AppShell.Current is AppShell shell)
                {
                    shell.SetUserLoggedOut();
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Error al cerrar sesión: {ex.Message}", "OK");
            }
        }
    }
}