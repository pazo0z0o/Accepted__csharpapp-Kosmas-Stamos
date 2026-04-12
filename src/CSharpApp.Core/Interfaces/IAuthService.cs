namespace CSharpApp.Core.Interfaces;

// Service for handling JWT authentication
public interface IAuthService
{
    //Service that handles JWT Authentication for our user
    Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    //Service that refreshes the access token using the refresh token
    Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default);
    // Service that retrieves the user's profile information using the access token
    Task<UserProfile?> GetProfileAsync(CancellationToken cancellationToken = default);
    //Gets the current access token (if authenticated)
    string? GetCurrentAccessToken();
    //Sets the access token for subsequent requests
    void SetAccessToken(string accessToken);
}