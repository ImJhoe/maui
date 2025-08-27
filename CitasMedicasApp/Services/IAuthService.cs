using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitasMedicasApp.Models;

namespace CitasMedicasApp.Services
{
    public interface IAuthService
    {
        Task<bool> LoginAsync(string username, string password);
        Task<bool> CambiarPasswordAsync(string username, string newPassword);
        Task<bool> LogoutAsync();
        Usuario? GetCurrentUser();
        bool IsLoggedIn();
        string GetCurrentUserRole();
    }
}
