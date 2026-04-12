namespace CSharpApp.Application.Products;

public class ProductsService : IProductsService
{

    private readonly IPlatziHttpClient _apiClient;
    private readonly RestApiSettings _restApiSettings;
    private readonly ILogger<ProductsService> _logger;

    public ProductsService(IPlatziHttpClient apiClient,IOptions<RestApiSettings> restApiSettings,ILogger<ProductsService> logger)
    {
        _apiClient = apiClient;
        _restApiSettings = restApiSettings?.Value;
        _logger = logger;
    }

    public async Task<IReadOnlyCollection<Product>> GetProducts()
    {
        try
        {
            _logger.LogInformation("Fetching all products from API");

            var products = await _apiClient.GetAsync<List<Product>>(_restApiSettings.Products!);

            if (products is null || products.Count == 0)
            {
                _logger.LogWarning("No products returned from API");
                return Array.Empty<Product>();
            }

            _logger.LogInformation("Successfully fetched {Count} products", products.Count);
            return products.AsReadOnly();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while fetching products");
            throw;
        }
    }
}