using CitasMedicasApp.Views;

namespace CitasMedicasApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Registrar rutas para navegación
        Routing.RegisterRoute("login", typeof(LoginPage));
        Routing.RegisterRoute("mainmenu", typeof(MainMenuPage));
        Routing.RegisterRoute("cambiarpassword", typeof(CambiarPasswordPage));
        Routing.RegisterRoute("historial", typeof(HistorialPage));
        Routing.RegisterRoute("citas", typeof(CitasPage));
        Routing.RegisterRoute("config", typeof(ConfigPage));
        // ConsultarCitasPage es opcional
        Routing.RegisterRoute("consultarcitas", typeof(ConsultarCitasPage));
    }

    public void SetUserLoggedIn(string userRole)
    {
        // Mostrar tabs principales y ocultar login
        LoginShell.IsVisible = false;
        MainTabBar.IsVisible = true;

        // Mostrar/ocultar tabs según el rol
        ConfigureTabsForRole(userRole);
    }

    public void SetUserLoggedOut()
    {
        // Mostrar login y ocultar tabs principales
        MainTabBar.IsVisible = false;
        LoginShell.IsVisible = true;

        // Navegar al login
        Current.GoToAsync("//login");
    }

    private void ConfigureTabsForRole(string role)
    {
        // Configurar visibilidad de tabs según el rol
        switch (role.ToLower())
        {
            case "medico":
            case "doctor":
                HistorialTab.IsVisible = true;
                break;
            case "recepcionista":
                HistorialTab.IsVisible = true;
                break;
            case "paciente":
                HistorialTab.IsVisible = false; // Los pacientes no ven historial
                break;
            default:
                HistorialTab.IsVisible = false;
                break;
        }
    }
}