namespace CSharpApp.Core.Dtos.Requests;


public sealed class RefreshTokenRequest
{
    [JsonPropertyName("refreshToken")]
    public string RefreshToken { get; set; } = string.Empty;
}