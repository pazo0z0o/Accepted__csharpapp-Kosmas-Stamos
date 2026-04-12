using CSharpApp.Application.Auth;

namespace CSharpApp.Infrastructure.Configuration;

public static class DefaultConfiguration
{
    public static IServiceCollection AddDefaultConfiguration(this IServiceCollection services)
    {
        var serviceProvider = services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        services.Configure<RestApiSettings>(configuration!.GetSection(nameof(RestApiSettings)));
        services.Configure<HttpClientSettings>(configuration.GetSection(nameof(HttpClientSettings)));

        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(ProductsService).Assembly); 
            cfg.AddOpenBehavior(typeof(CSharpApp.Application.Common.Behaviours.LoggingBehaviour<,>)); 
        });


        //Turned to scoped for better performance, and release after use of the service - 
        services.AddScoped<IProductsService, ProductsService>();
        services.AddScoped<IAuthService, AuthService>();



        return services;
    }
}