using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Queries.GetCategoryById
{
    public sealed class GetCategoryByIdQueryHandler : IQueryHandler<GetCategoryByIdQuery, Category?>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<GetCategoryByIdQueryHandler> _logger;

        public GetCategoryByIdQueryHandler(
            IPlatziHttpClient apiClient,
            IOptions<RestApiSettings> restApiSettings,
            ILogger<GetCategoryByIdQueryHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value;
            _logger = logger;
        }

        public async Task<Category?> Handle(
            GetCategoryByIdQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching category with ID: {CategoryId}", request.CategoryId);

            var endpoint = $"{_restApiSettings.Categories}/{request.CategoryId}";

            var category = await _apiClient.GetAsync<Category>(endpoint, cancellationToken);

            if (category is null)
            {
                _logger.LogWarning("Category with ID {CategoryId} not found", request.CategoryId);
                return null;
            }

            _logger.LogInformation(
                "Successfully fetched category {CategoryId}: {CategoryName}",
                category.Id,
                category.Name);

            return category;
        }
    }
}
