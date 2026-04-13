using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Queries.GetProductById
{
    public sealed class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, Product?>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<GetProductByIdQueryHandler> _logger;

        public GetProductByIdQueryHandler(IPlatziHttpClient apiClient,IOptions<RestApiSettings> restApiSettings,ILogger<GetProductByIdQueryHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value ;
            _logger = logger;
        }

        public async Task<Product?> Handle(
            GetProductByIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching product with ID: {ProductId}", request.ProductId);

            var endpoint = $"{_restApiSettings.Products}/{request.ProductId}";

            var product = await _apiClient.GetAsync<Product>(endpoint, cancellationToken);

            if (product is null)
            {
                _logger.LogWarning("Product with ID {ProductId} not found", request.ProductId);
                return null;
            }

            _logger.LogInformation(
                "Successfully fetched product {ProductId}: {ProductTitle}",
                product.Id,
                product.Title);

            return product;
        }
    }
}
