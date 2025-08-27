using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class Usuario
{
    [JsonPropertyName("id_usuario")]
    public int IdUsuario { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("nombres")]
    public string Nombres { get; set; } = string.Empty;

    [JsonPropertyName("apellidos")]
    public string Apellidos { get; set; } = string.Empty;

    [JsonPropertyName("correo")]
    public string Correo { get; set; } = string.Empty;

    [JsonPropertyName("cedula")]
    public string Cedula { get; set; } = string.Empty;

    [JsonPropertyName("nombre_rol")]
    public string NombreRol { get; set; } = string.Empty;

    [JsonPropertyName("nombre_estado")]
    public string NombreEstado { get; set; } = string.Empty;

    public string NombreCompleto => $"{Nombres} {Apellidos}";
}