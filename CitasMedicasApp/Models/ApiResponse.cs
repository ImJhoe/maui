using System.Text.Json.Serialization;

namespace CitasMedicasApp.Models;

public class ApiResponse<T>
{
    [JsonPropertyName("success")]
    public bool Success { get; set; }

    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("data")]
    public T? Data { get; set; }

    [JsonPropertyName("code")]
    public string? Code { get; set; }

    [JsonPropertyName("total")]
    public int? Total { get; set; }
}

public class LoginResponse
{
    [JsonPropertyName("user")]
    public Usuario User { get; set; } = new();

    [JsonPropertyName("token")]
    public string? Token { get; set; }
}