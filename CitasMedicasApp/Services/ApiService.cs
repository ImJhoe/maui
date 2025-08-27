using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using CitasMedicasApp.Helpers;
using CitasMedicasApp.Models;

namespace CitasMedicasApp.Services;

public class ApiService : IApiService
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public ApiService()
    {
        _httpClient = new HttpClient()
        {
            BaseAddress = new Uri(Constants.BaseApiUrl),
            Timeout = TimeSpan.FromSeconds(Constants.TimeoutSeconds)
        };

        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        };

        // Headers por defecto
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "CitasMedicasApp/1.0");
    }

    private void SetAuthHeader()
    {
        if (!string.IsNullOrEmpty(Settings.AuthToken))
        {
            _httpClient.DefaultRequestHeaders.Authorization =
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", Settings.AuthToken);
        }
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(string username, string password)
    {
        try
        {
            var loginData = new
            {
                username = username,
                password = password
            };

            var json = JsonSerializer.Serialize(loginData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.LoginEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var apiResponse = JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(responseContent, _jsonOptions);
                return apiResponse ?? new ApiResponse<LoginResponse> { Success = false, Message = "Respuesta vacía del servidor" };
            }
            else
            {
                var errorResponse = JsonSerializer.Deserialize<ApiResponse<LoginResponse>>(responseContent, _jsonOptions);
                return errorResponse ?? new ApiResponse<LoginResponse> { Success = false, Message = "Error desconocido del servidor" };
            }
        }
        catch (HttpRequestException )
        {
            return new ApiResponse<LoginResponse>
            {
                Success = false,
                Message = "Error de conexión: Verifique su conexión a internet y que el servidor esté funcionando",
                Code = "CONNECTION_ERROR"
            };
        }
        catch (TaskCanceledException)
        {
            return new ApiResponse<LoginResponse>
            {
                Success = false,
                Message = "La solicitud tardó demasiado tiempo. Intente nuevamente",
                Code = "TIMEOUT_ERROR"
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<LoginResponse>
            {
                Success = false,
                Message = $"Error inesperado: {ex.Message}",
                Code = "UNEXPECTED_ERROR"
            };
        }
    }

    public async Task<ApiResponse<object>> CambiarPasswordAsync(string username, string newPassword)
    {
        try
        {
            SetAuthHeader();

            var changePasswordData = new
            {
                username = username,
                new_password = newPassword
            };

            var json = JsonSerializer.Serialize(changePasswordData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.CambiarPasswordEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<object>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<object> { Success = false, Message = "Respuesta vacía del servidor" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<object>
            {
                Success = false,
                Message = $"Error al cambiar contraseña: {ex.Message}",
                Code = "CHANGE_PASSWORD_ERROR"
            };
        }
    }

    public async Task<ApiResponse<List<HistorialClinico>>> BuscarHistorialPorCedulaAsync(string cedula)
    {
        try
        {
            SetAuthHeader();

            var requestData = new { cedula = cedula };
            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.BuscarHistorialEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<HistorialClinico>>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<List<HistorialClinico>>
            {
                Success = false,
                Message = "Respuesta vacía del servidor",
                Data = new List<HistorialClinico>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<HistorialClinico>>
            {
                Success = false,
                Message = $"Error al buscar historial: {ex.Message}",
                Data = new List<HistorialClinico>(),
                Code = "HISTORIAL_ERROR"
            };
        }
    }

    public async Task<ApiResponse<List<Cita>>> ConsultarCitasPorEspecialidadYMedicoAsync(int? especialidadId, int? medicoId)
    {
        try
        {
            SetAuthHeader();

            var requestData = new
            {
                id_especialidad = especialidadId,
                id_medico = medicoId
            };

            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.ConsultarCitasEspecialidadEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Cita>>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<List<Cita>>
            {
                Success = false,
                Message = "Respuesta vacía del servidor",
                Data = new List<Cita>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Cita>>
            {
                Success = false,
                Message = $"Error al consultar citas: {ex.Message}",
                Data = new List<Cita>(),
                Code = "CITAS_ERROR"
            };
        }
    }

    public async Task<ApiResponse<List<Cita>>> ConsultarCitasPorRangoFechasAsync(string fechaInicio, string fechaFin, int? medicoId = null)
    {
        try
        {
            SetAuthHeader();

            var requestData = new
            {
                fecha_inicio = fechaInicio,
                fecha_fin = fechaFin,
                id_medico = medicoId
            };

            var json = JsonSerializer.Serialize(requestData, _jsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(Constants.ConsultarCitasFechasEndpoint, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Cita>>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<List<Cita>>
            {
                Success = false,
                Message = "Respuesta vacía del servidor",
                Data = new List<Cita>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Cita>>
            {
                Success = false,
                Message = $"Error al consultar citas por fechas: {ex.Message}",
                Data = new List<Cita>(),
                Code = "CITAS_FECHAS_ERROR"
            };
        }
    }

    public async Task<ApiResponse<List<Especialidad>>> ObtenerEspecialidadesAsync()
    {
        try
        {
            SetAuthHeader();

            var response = await _httpClient.GetAsync(Constants.EspecialidadesEndpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Especialidad>>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<List<Especialidad>>
            {
                Success = false,
                Message = "Respuesta vacía del servidor",
                Data = new List<Especialidad>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Especialidad>>
            {
                Success = false,
                Message = $"Error al obtener especialidades: {ex.Message}",
                Data = new List<Especialidad>(),
                Code = "ESPECIALIDADES_ERROR"
            };
        }
    }

    public async Task<ApiResponse<List<Medico>>> ObtenerMedicosAsync()
    {
        try
        {
            SetAuthHeader();

            var response = await _httpClient.GetAsync(Constants.MedicosEndpoint);
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = JsonSerializer.Deserialize<ApiResponse<List<Medico>>>(responseContent, _jsonOptions);
            return apiResponse ?? new ApiResponse<List<Medico>>
            {
                Success = false,
                Message = "Respuesta vacía del servidor",
                Data = new List<Medico>()
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<List<Medico>>
            {
                Success = false,
                Message = $"Error al obtener médicos: {ex.Message}",
                Data = new List<Medico>(),
                Code = "MEDICOS_ERROR"
            };
        }
    }

    public async Task<bool> TestConnectionAsync()
    {
        try
        {
            var response = await _httpClient.GetAsync("/");
            return response.IsSuccessStatusCode;
        }
        catch
        {
            return false;
        }
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}