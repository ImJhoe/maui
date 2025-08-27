using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class HistorialClinico
{
    [JsonPropertyName("id_historial")]
    public int IdHistorial { get; set; }

    [JsonPropertyName("paciente_nombre")]
    public string PacienteNombre { get; set; } = string.Empty;

    [JsonPropertyName("paciente_cedula")]
    public string PacienteCedula { get; set; } = string.Empty;

    [JsonPropertyName("fecha_nacimiento")]
    public DateTime FechaNacimiento { get; set; }

    [JsonPropertyName("tipo_sangre")]
    public string? TipoSangre { get; set; }

    [JsonPropertyName("alergias")]
    public string? Alergias { get; set; }

    [JsonPropertyName("antecedentes_medicos")]
    public string? AntecedentesMedicos { get; set; }

    [JsonPropertyName("contacto_emergencia")]
    public string? ContactoEmergencia { get; set; }

    [JsonPropertyName("telefono_emergencia")]
    public string? TelefonoEmergencia { get; set; }

    public int Edad => DateTime.Now.Year - FechaNacimiento.Year -
                      (DateTime.Now.DayOfYear < FechaNacimiento.DayOfYear ? 1 : 0);
}