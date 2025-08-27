using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class Medico
{
    [JsonPropertyName("id_doctor")]
    public int IdDoctor { get; set; }

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = string.Empty;

    [JsonPropertyName("apellidos")]
    public string Apellidos { get; set; } = string.Empty;

    [JsonPropertyName("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [JsonPropertyName("especialidad_nombre")]
    public string? EspecialidadNombre { get; set; }

    [JsonPropertyName("telefono")]
    public string? Telefono { get; set; }

    [JsonPropertyName("correo")]
    public string? Correo { get; set; }

    public string NombreCompleto => $"Dr. {Nombres} {Apellidos}";
}