using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Queries.GetAllProducts
{
    public sealed class GetAllProductsQueryHandler : IQueryHandler<GetAllProductsQuery, IReadOnlyCollection<Product>>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<GetAllProductsQueryHandler> _logger;

        public GetAllProductsQueryHandler(IPlatziHttpClient apiClient,IOptions<RestApiSettings> restApiSettings,ILogger<GetAllProductsQueryHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Product>> Handle(
            GetAllProductsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all products from API");

            var products = await _apiClient.GetAsync<List<Product>>(
                _restApiSettings.Products!,
                cancellationToken);

            if (products is null || products.Count == 0)
            {
                _logger.LogWarning("No products returned from API");
                return Array.Empty<Product>();
            }

            _logger.LogInformation("Successfully fetched {Count} products", products.Count);
            return products.AsReadOnly();
        }
    }
}
