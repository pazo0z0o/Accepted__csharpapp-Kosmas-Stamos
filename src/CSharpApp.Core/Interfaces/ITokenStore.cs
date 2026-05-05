namespace CSharpApp.Core.Interfaces;

/// <summary>
/// Thread-safe singleton store for the current bearer token.
/// Decouples token lifetime from the scoped AuthService.
/// </summary>
public interface ITokenStore
{
    string? AccessToken { get; }
    void SetToken(string accessToken);
    void ClearToken();
}
