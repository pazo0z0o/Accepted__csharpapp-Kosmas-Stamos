namespace CSharpApp.Infrastructure.Configuration;

public static class HttpConfiguration
{

    public static IServiceCollection AddHttpConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();
        var httpClientSettings = configuration!.GetSection(nameof(HttpClientSettings)).Get<HttpClientSettings>();
        var restApiSettings = configuration.GetSection(nameof(RestApiSettings)).Get<RestApiSettings>();

        if (httpClientSettings is null)
        {
            throw new InvalidOperationException("HttpClientSettings not pulled from appsettings.json");
        }

        if (restApiSettings is null)
        {
            throw new InvalidOperationException("RestApiSettings not pulled from appsettings.json");
        }

        // Register typed HttpClient for Platzi API
        services.AddHttpClient<IPlatziHttpClient, PlatziHttpClient>(client =>
        {
            client.BaseAddress = new Uri(restApiSettings.BaseUrl!);
            client.Timeout = TimeSpan.FromSeconds(30);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("User-Agent", "CSharpApp/1.0");
        })
        .SetHandlerLifetime(TimeSpan.FromMinutes(httpClientSettings.LifeTime))
        .AddResilienceHandler("retry", builder =>
        {
            builder.AddRetry(new HttpRetryStrategyOptions
            {       
                MaxRetryAttempts = httpClientSettings.RetryCount,
                Delay = TimeSpan.FromMilliseconds(httpClientSettings.SleepDuration),
                BackoffType = DelayBackoffType.Exponential,
                UseJitter = true,
                ShouldHandle = args => ValueTask.FromResult(
                    args.Outcome.Result?.IsSuccessStatusCode == false ||
                    args.Outcome.Exception is HttpRequestException)
            });
        });

        return services;
    }
}