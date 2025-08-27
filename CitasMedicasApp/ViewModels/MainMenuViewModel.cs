using System.Windows.Input;
using CitasMedicasApp.Models;
using CitasMedicasApp.Services;

namespace CitasMedicasApp.ViewModels;

public class MainMenuViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private Usuario? _currentUser;
    private string _welcomeMessage = string.Empty;
    private bool _canAccessHistorial;
    private bool _canAccessCitas = true;

    public MainMenuViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Menú Principal";

        LoadUserInfo();

        // Comandos
        LogoutCommand = new Command(async () => await LogoutAsync());
        GoToHistorialCommand = new Command(async () => await GoToHistorialAsync(), () => CanAccessHistorial);
        GoToCitasCommand = new Command(async () => await GoToCitasAsync(), () => CanAccessCitas);
        GoToConfigCommand = new Command(async () => await GoToConfigAsync());
        CambiarPasswordCommand = new Command(async () => await CambiarPasswordAsync());
    }

    public Usuario? CurrentUser
    {
        get => _currentUser;
        set => SetProperty(ref _currentUser, value);
    }

    public string WelcomeMessage
    {
        get => _welcomeMessage;
        set => SetProperty(ref _welcomeMessage, value);
    }

    public bool CanAccessHistorial
    {
        get => _canAccessHistorial;
        set => SetProperty(ref _canAccessHistorial, value);
    }

    public bool CanAccessCitas
    {
        get => _canAccessCitas;
        set => SetProperty(ref _canAccessCitas, value);
    }

    public ICommand LogoutCommand { get; }
    public ICommand GoToHistorialCommand { get; }
    public ICommand GoToCitasCommand { get; }
    public ICommand GoToConfigCommand { get; }
    public ICommand CambiarPasswordCommand { get; }

    private void LoadUserInfo()
    {
        CurrentUser = _authService.GetCurrentUser();

        if (CurrentUser != null)
        {
            WelcomeMessage = $"Bienvenido, {CurrentUser.NombreCompleto}";

            // Configurar permisos según el rol
            var role = CurrentUser.NombreRol.ToLower();
            CanAccessHistorial = role == "medico" || role == "doctor" || role == "recepcionista";
            CanAccessCitas = true; // Todos pueden ver citas
        }
    }

    private async Task LogoutAsync()
    {
        var confirm = await ShowConfirmAsync("Cerrar Sesión", "¿Está seguro que desea cerrar sesión?");

        if (confirm)
        {
            await _authService.LogoutAsync();

            // Configurar AppShell para usuario deslogueado
            if (AppShell.Current is AppShell shell)
            {
                shell.SetUserLoggedOut();
            }
        }
    }

    private async Task GoToHistorialAsync()
    {
        if (CanAccessHistorial)
        {
            await Shell.Current.GoToAsync("historial");
        }
    }

    private async Task GoToCitasAsync()
    {
        if (CanAccessCitas)
        {
            await Shell.Current.GoToAsync("citas");
        }
    }

    private async Task GoToConfigAsync()
    {
        await Shell.Current.GoToAsync("config");
    }

    private async Task CambiarPasswordAsync()
    {
        await Shell.Current.GoToAsync("cambiarpassword");
    }
}