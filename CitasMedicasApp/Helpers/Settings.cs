using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicasApp.Helpers
{
    public static class Settings
    {
        public static string AuthToken
        {
            get => Preferences.Get(Constants.TokenKey, string.Empty);
            set => Preferences.Set(Constants.TokenKey, value);
        }

        public static string UserData
        {
            get => Preferences.Get(Constants.UserDataKey, string.Empty);
            set => Preferences.Set(Constants.UserDataKey, value);
        }

        public static bool IsLoggedIn
        {
            get => Preferences.Get(Constants.IsLoggedInKey, false);
            set => Preferences.Set(Constants.IsLoggedInKey, value);
        }

        public static void ClearUserData()
        {
            Preferences.Remove(Constants.TokenKey);
            Preferences.Remove(Constants.UserDataKey);
            Preferences.Remove(Constants.IsLoggedInKey);
        }
    }
}
