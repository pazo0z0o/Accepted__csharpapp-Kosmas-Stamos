namespace CSharpApp.Core.Interfaces;

/// <summary>
/// HTTP client abstraction for Platzi Fake Store API
/// </summary>
public interface IPlatziHttpClient
{

    /// <typeparam name="T">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint followed by the relative path)</param>
    /// <returns>response object</returns>
    Task<T?> GetAsync<T>(string endpoint, CancellationToken cancellationToken = default);


    /// <typeparam name="TRequest">Request body type</typeparam>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint (relative path)</param>
    /// <param name="request">Request body </param>
    /// <returns>response object</returns>
    Task<TResponse?> PostAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);

  
    /// <typeparam name="TRequest">Request body type</typeparam>
    /// <typeparam name="TResponse">Expected response type</typeparam>
    /// <param name="endpoint">API endpoint (relative path)</param>
    /// <param name="request">Request body object</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>response object</returns>
    Task<TResponse?> PutAsync<TRequest, TResponse>(string endpoint, TRequest request, CancellationToken cancellationToken = default);

   
    /// <param name="endpoint">API endpoint (relative path)</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if successful</returns>
    Task<bool> DeleteAsync(string endpoint, CancellationToken cancellationToken = default);
}