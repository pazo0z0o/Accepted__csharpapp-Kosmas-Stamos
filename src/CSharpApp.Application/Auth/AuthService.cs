namespace CSharpApp.Application.Auth;

/// <summary>
/// Service for handling JWT authentication with Platzi API
/// </summary>
public class AuthService : IAuthService
{
    private readonly IPlatziHttpClient _apiClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<AuthService> _logger;
    private string? _currentAccessToken;

    public AuthService(IPlatziHttpClient apiClient,IOptions<RestApiSettings> restApiSettings,ILogger<AuthService> logger)
    {
        _apiClient = apiClient ;
        _restApiSettings = restApiSettings?.Value ;
        _logger = logger;
    }

    public async Task<AuthResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting to login with email: {Email}", request.Email);

            var response = await _apiClient.PostAsync<LoginRequest, AuthResponse>(_restApiSettings.Auth!,request,cancellationToken);

            if (response is not null && !string.IsNullOrEmpty(response.AccessToken))
            {
                _currentAccessToken = response.AccessToken;
                _logger.LogInformation("Login successful for email: {Email}", request.Email);
            }
            else
            {
                _logger.LogWarning("Login failed - no access token received");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for email: {Email}", request.Email);
            throw;
        }
    }

    public async Task<AuthResponse?> RefreshTokenAsync(RefreshTokenRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Attempting to refresh access token");

            var response = await _apiClient.PostAsync<RefreshTokenRequest, AuthResponse>(
                "auth/refresh-token",
                request,
                cancellationToken);

            if (response is not null && !string.IsNullOrEmpty(response.AccessToken))
            {
                _currentAccessToken = response.AccessToken;
                _logger.LogInformation("Token refresh successful");
            }

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token refresh failed");
            throw;
        }
    }

    public async Task<UserProfile?> GetProfileAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrEmpty(_currentAccessToken))
            {
                _logger.LogWarning("Cannot get profile - not authenticated");
                return null;
            }

            _logger.LogInformation("Fetching user profile");

            var profile = await _apiClient.GetAsync<UserProfile>(
                "auth/profile",
                cancellationToken);

            _logger.LogInformation("Profile fetched successfully for user: {UserId}", profile?.Id);
            return profile;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to fetch user profile");
            throw;
        }
    }

    public string? GetCurrentAccessToken()
    {
        return _currentAccessToken;
    }

    public void SetAccessToken(string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        _currentAccessToken = accessToken;
        _logger.LogDebug("Access token updated");
    }
}
