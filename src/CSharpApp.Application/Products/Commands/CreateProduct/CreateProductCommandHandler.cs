using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Products.Commands.CreateProduct
{
    public sealed class CreateProductCommandHandler : ICommandHandler<CreateProductCommand, Product?>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(IPlatziHttpClient apiClient,IOptions<RestApiSettings> restApiSettings,ILogger<CreateProductCommandHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value;
            _logger = logger;
        }

        public async Task<Product?> Handle(
            CreateProductCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new product: {ProductTitle}", request.Title);

            // Map command to API request
            var apiRequest = new CreateProductRequest
            {
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                CategoryId = request.CategoryId,
                Images = request.Images
            };

            var createdProduct = await _apiClient.PostAsync<CreateProductRequest, Product>(
                _restApiSettings.Products!,
                apiRequest,
                cancellationToken);

            if (createdProduct is null)
            {
                _logger.LogWarning("Failed to create product: {ProductTitle}", request.Title);
                return null;
            }

            _logger.LogInformation(
                "Successfully created product {ProductId}: {ProductTitle}",
                createdProduct.Id,
                createdProduct.Title);

            return createdProduct;
        }
    }
}
