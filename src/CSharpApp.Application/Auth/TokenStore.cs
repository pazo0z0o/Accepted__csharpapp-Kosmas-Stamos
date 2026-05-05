namespace CSharpApp.Application.Auth;

public sealed class TokenStore : ITokenStore
{
    private volatile string? _accessToken;

    public string? AccessToken => _accessToken;

    public void SetToken(string accessToken)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(accessToken);
        _accessToken = accessToken;
    }

    public void ClearToken() => _accessToken = null;
}
