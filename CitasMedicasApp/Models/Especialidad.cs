using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class Especialidad
{
    [JsonPropertyName("id_especialidad")]
    public int IdEspecialidad { get; set; }

    [JsonPropertyName("nombre_especialidad")]
    public string NombreEspecialidad { get; set; } = string.Empty;

    [JsonPropertyName("descripcion")]
    public string? Descripcion { get; set; }
}