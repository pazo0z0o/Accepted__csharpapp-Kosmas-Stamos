using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSharpApp.Application.Categories.Commands.CreateCategory
{
    public sealed class CreateCategoryCommandHandler : ICommandHandler<CreateCategoryCommand, Category?>
    {
        private readonly IPlatziHttpClient _apiClient;
        private readonly RestApiSettings _restApiSettings;
        private readonly ILogger<CreateCategoryCommandHandler> _logger;

        public CreateCategoryCommandHandler(
            IPlatziHttpClient apiClient,
            IOptions<RestApiSettings> restApiSettings,
            ILogger<CreateCategoryCommandHandler> logger)
        {
            _apiClient = apiClient;
            _restApiSettings = restApiSettings?.Value;
            _logger = logger;
        }

        public async Task<Category?> Handle(
            CreateCategoryCommand request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("Creating new category: {CategoryName}", request.Name);

            var apiRequest = new CreateCategoryRequest
            {
                Name = request.Name,
                Image = request.Image
            };

            var createdCategory = await _apiClient.PostAsync<CreateCategoryRequest, Category>(
                _restApiSettings.Categories!,
                apiRequest,
                cancellationToken);

            if (createdCategory is null)
            {
                _logger.LogWarning("Failed to create category: {CategoryName}", request.Name);
                return null;
            }

            _logger.LogInformation(
                "Successfully created category {CategoryId}: {CategoryName}",
                createdCategory.Id,
                createdCategory.Name);

            return createdCategory;
        }
    }
}
