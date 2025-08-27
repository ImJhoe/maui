using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicasApp.Helpers
{
    public static class Constants
    {
        // Configuración de la API
        public const string BaseApiUrl = "http://10.0.2.2:8081"; // Para Android Emulator
                                                                 // public const string BaseApiUrl = "http://localhost:8081"; // Para Windows/Mac
                                                                 // public const string BaseApiUrl = "http://192.168.1.100:8081"; // Para dispositivos físicos

        // Endpoints
        public const string LoginEndpoint = "/auth/login";
        public const string CambiarPasswordEndpoint = "/auth/cambiar-password";
        public const string BuscarHistorialEndpoint = "/historial/buscar-cedula";
        public const string ConsultarCitasEspecialidadEndpoint = "/citas/consultar-especialidad-medico";
        public const string ConsultarCitasFechasEndpoint = "/citas/consultar-rango-fechas";
        public const string EspecialidadesEndpoint = "/consultas/especialidades";
        public const string MedicosEndpoint = "/consultas/medicos";

        // Configuración de timeout
        public const int TimeoutSeconds = 30;

        // Claves para el almacenamiento local
        public const string TokenKey = "auth_token";
        public const string UserDataKey = "user_data";
        public const string IsLoggedInKey = "is_logged_in";
    }
}
