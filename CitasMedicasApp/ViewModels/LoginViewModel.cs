using System.Windows.Input;
using CitasMedicasApp.Services;

namespace CitasMedicasApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly IApiService _apiService;
    private string _username = string.Empty;
    private string _password = string.Empty;
    private bool _isServerConnected = true;

    public LoginViewModel(IAuthService authService, IApiService apiService)
    {
        _authService = authService;
        _apiService = apiService;
        Title = "Iniciar Sesión";

        LoginCommand = new Command(async () => await LoginAsync(), () => CanLogin());
        TestConnectionCommand = new Command(async () => await TestConnectionAsync());

        // Test connection on load
        _ = Task.Run(async () => await TestConnectionAsync());
    }

    public string Username
    {
        get => _username;
        set
        {
            SetProperty(ref _username, value);
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public string Password
    {
        get => _password;
        set
        {
            SetProperty(ref _password, value);
            ((Command)LoginCommand).ChangeCanExecute();
        }
    }

    public bool IsServerConnected
    {
        get => _isServerConnected;
        set => SetProperty(ref _isServerConnected, value);
    }

    public ICommand LoginCommand { get; }
    public ICommand TestConnectionCommand { get; }

    private bool CanLogin()
    {
        return !IsBusy &&
               !string.IsNullOrWhiteSpace(Username) &&
               !string.IsNullOrWhiteSpace(Password) &&
               IsServerConnected;
    }

    private async Task LoginAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Validaciones básicas
            if (Username.Length < 3)
            {
                await ShowAlertAsync("Error", "El nombre de usuario debe tener al menos 3 caracteres");
                return;
            }

            if (Password.Length < 3)
            {
                await ShowAlertAsync("Error", "La contraseña debe tener al menos 3 caracteres");
                return;
            }

            var success = await _authService.LoginAsync(Username, Password);

            if (success)
            {
                var currentUser = _authService.GetCurrentUser();

                // Configurar AppShell para el rol del usuario
                if (AppShell.Current is AppShell shell)
                {
                    shell.SetUserLoggedIn(currentUser?.NombreRol ?? "");
                }

                // Navegar al menú principal
                await Shell.Current.GoToAsync("//mainmenu");
            }
            else
            {
                await ShowAlertAsync("Error de Autenticación",
                    "Usuario o contraseña incorrectos. Verifique sus credenciales e intente nuevamente.");
            }
        }
        catch (Exception ex)
        {
            await ShowAlertAsync("Error", $"Error inesperado: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task TestConnectionAsync()
    {
        try
        {
            IsServerConnected = await _apiService.TestConnectionAsync();
        }
        catch
        {
            IsServerConnected = false;
        }
    }
}