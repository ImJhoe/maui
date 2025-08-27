using System.Windows.Input;
using CitasMedicasApp.Services;

namespace CitasMedicasApp.ViewModels;

public class CambiarPasswordViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _currentPassword = string.Empty;
    private string _newPassword = string.Empty;
    private string _confirmPassword = string.Empty;

    public CambiarPasswordViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Cambiar Contraseña";

        CambiarPasswordCommand = new Command(async () => await CambiarPasswordAsync(), CanCambiarPassword);
        VolverCommand = new Command(async () => await VolverAsync());
    }

    public string CurrentPassword
    {
        get => _currentPassword;
        set
        {
            SetProperty(ref _currentPassword, value);
            ((Command)CambiarPasswordCommand).ChangeCanExecute();
        }
    }

    public string NewPassword
    {
        get => _newPassword;
        set
        {
            SetProperty(ref _newPassword, value);
            ((Command)CambiarPasswordCommand).ChangeCanExecute();
        }
    }

    public string ConfirmPassword
    {
        get => _confirmPassword;
        set
        {
            SetProperty(ref _confirmPassword, value);
            ((Command)CambiarPasswordCommand).ChangeCanExecute();
        }
    }

    public ICommand CambiarPasswordCommand { get; }
    public ICommand VolverCommand { get; }

    private bool CanCambiarPassword()
    {
        return !IsBusy &&
               !string.IsNullOrWhiteSpace(CurrentPassword) &&
               !string.IsNullOrWhiteSpace(NewPassword) &&
               !string.IsNullOrWhiteSpace(ConfirmPassword) &&
               NewPassword == ConfirmPassword &&
               NewPassword.Length >= 6;
    }

    private async Task CambiarPasswordAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;

            // Validaciones
            if (NewPassword != ConfirmPassword)
            {
                await ShowAlertAsync("Error", "Las contraseñas no coinciden");
                return;
            }

            if (NewPassword.Length < 6)
            {
                await ShowAlertAsync("Error", "La nueva contraseña debe tener al menos 6 caracteres");
                return;
            }

            if (CurrentPassword == NewPassword)
            {
                await ShowAlertAsync("Error", "La nueva contraseña debe ser diferente a la actual");
                return;
            }

            var currentUser = _authService.GetCurrentUser();
            if (currentUser == null)
            {
                await ShowAlertAsync("Error", "No se pudo obtener la información del usuario");
                return;
            }

            var success = await _authService.CambiarPasswordAsync(currentUser.Username, NewPassword);

            if (success)
            {
                await ShowAlertAsync("Éxito", "Contraseña cambiada correctamente");

                // Limpiar campos
                CurrentPassword = string.Empty;
                NewPassword = string.Empty;
                ConfirmPassword = string.Empty;

                // Volver al menú principal
                await Shell.Current.GoToAsync("..");
            }
            else
            {
                await ShowAlertAsync("Error", "No se pudo cambiar la contraseña. Verifique su contraseña actual.");
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

    private async Task VolverAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}