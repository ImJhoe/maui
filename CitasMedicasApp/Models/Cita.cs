using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class Cita
{
    [JsonPropertyName("id_cita")]
    public int IdCita { get; set; }

    [JsonPropertyName("fecha_hora")]
    public DateTime FechaHora { get; set; }

    [JsonPropertyName("estado")]
    public string Estado { get; set; } = string.Empty;

    [JsonPropertyName("observaciones")]
    public string? Observaciones { get; set; }

    [JsonPropertyName("paciente_nombre")]
    public string PacienteNombre { get; set; } = string.Empty;

    [JsonPropertyName("paciente_cedula")]
    public string PacienteCedula { get; set; } = string.Empty;

    [JsonPropertyName("medico_nombre")]
    public string MedicoNombre { get; set; } = string.Empty;

    [JsonPropertyName("especialidad_nombre")]
    public string EspecialidadNombre { get; set; } = string.Empty;

    [JsonPropertyName("sucursal_nombre")]
    public string SucursalNombre { get; set; } = string.Empty;

    public string FechaHoraFormatted => FechaHora.ToString("dd/MM/yyyy HH:mm");
}