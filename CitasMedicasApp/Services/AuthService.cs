using System.Text.Json;
using CitasMedicasApp.Helpers;
using CitasMedicasApp.Models;

namespace CitasMedicasApp.Services;

public class AuthService : IAuthService
{
    private readonly IApiService _apiService;
    private Usuario? _currentUser;

    public AuthService(IApiService apiService)
    {
        _apiService = apiService;
        LoadUserFromStorage();
    }

    public async Task<bool> LoginAsync(string username, string password)
    {
        try
        {
            var response = await _apiService.LoginAsync(username, password);

            if (response.Success && response.Data?.User != null)
            {
                _currentUser = response.Data.User;

                // Guardar datos del usuario
                var userData = JsonSerializer.Serialize(_currentUser);
                Settings.UserData = userData;
                Settings.AuthToken = response.Data.Token ?? string.Empty;
                Settings.IsLoggedIn = true;

                return true;
            }

            return false;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> CambiarPasswordAsync(string username, string newPassword)
    {
        try
        {
            var response = await _apiService.CambiarPasswordAsync(username, newPassword);
            return response.Success;
        }
        catch (Exception)
        {
            return false;
        }
    }

    public async Task<bool> LogoutAsync()
    {
        try
        {
            // Limpiar datos locales
            _currentUser = null;
            Settings.ClearUserData();

            return await Task.FromResult(true);
        }
        catch (Exception)
        {
            return false;
        }
    }

    public Usuario? GetCurrentUser()
    {
        return _currentUser;
    }

    public bool IsLoggedIn()
    {
        return Settings.IsLoggedIn && _currentUser != null;
    }

    public string GetCurrentUserRole()
    {
        return _currentUser?.NombreRol ?? string.Empty;
    }

    private void LoadUserFromStorage()
    {
        try
        {
            if (Settings.IsLoggedIn && !string.IsNullOrEmpty(Settings.UserData))
            {
                _currentUser = JsonSerializer.Deserialize<Usuario>(Settings.UserData);
            }
        }
        catch (Exception)
        {
            // Si hay error cargando datos, limpiar todo
            Settings.ClearUserData();
        }
    }
}