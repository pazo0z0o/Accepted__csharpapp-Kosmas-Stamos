using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Queries.GetAllCategories
{
    public sealed class GetAllCategoriesQueryHandler : IQueryHandler<GetAllCategoriesQuery, IReadOnlyCollection<Category>>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<GetAllCategoriesQueryHandler> _logger;

        public GetAllCategoriesQueryHandler(IPlatziHttpClient apiClient, IOptions<RestApiSettings> restApiSettings,ILogger<GetAllCategoriesQueryHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value;
            _logger = logger;
        }

        public async Task<IReadOnlyCollection<Category>> Handle(GetAllCategoriesQuery request,CancellationToken cancellationToken)
        {
            _logger.LogInformation("Fetching all categories from API");

            var categories = await _apiClient.GetAsync<List<Category>>(
                _restApiSettings.Categories!,
                cancellationToken);

            if (categories is null || categories.Count == 0)
            {
                _logger.LogWarning("No categories returned from API");
                return Array.Empty<Category>();
            }

            _logger.LogInformation("Successfully fetched {Count} categories", categories.Count);
            return categories.AsReadOnly();
        }
    }
}
