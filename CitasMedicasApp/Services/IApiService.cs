using CitasMedicasApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CitasMedicasApp.Services
{
    public interface IApiService
    {
        Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password);
        Task<ApiResponse<object>> CambiarPasswordAsync(string username, string newPassword);
        Task<ApiResponse<List<HistorialClinico>>> BuscarHistorialPorCedulaAsync(string cedula);
        Task<ApiResponse<List<Cita>>> ConsultarCitasPorEspecialidadYMedicoAsync(int? especialidadId, int? medicoId);
        Task<ApiResponse<List<Cita>>> ConsultarCitasPorRangoFechasAsync(string fechaInicio, string fechaFin, int? medicoId = null);
        Task<ApiResponse<List<Especialidad>>> ObtenerEspecialidadesAsync();
        Task<ApiResponse<List<Medico>>> ObtenerMedicosAsync();
        Task<bool> TestConnectionAsync();
    }
}
