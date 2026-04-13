using CSharpApp.Api.Middleware;
using CSharpApp.Application.Categories.Commands.CreateCategory;
using CSharpApp.Application.Categories.Queries.GetAllCategories;
using CSharpApp.Application.Categories.Queries.GetCategoryById;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).CreateLogger();
builder.Logging.ClearProviders().AddSerilog(logger);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDefaultConfiguration();
builder.Services.AddHttpConfiguration();
builder.Services.AddProblemDetails();
builder.Services.AddApiVersioning();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
// Add performance monitoring middleware
app.UseMiddleware<RequestPerformanceMiddleware>();

var versionedEndpointRouteBuilder = app.NewVersionedApi();


// PRODUCTS ENDPOINTS
// ========================================

// Get all products - changed for 
versionedEndpointRouteBuilder
    .MapGet("api/v{version:apiVersion}/products", async (IMediator mediator) =>
    {
        var query = new GetAllProductsQuery();
        return await mediator.Send(query);
    })
    .WithName("GetAllProducts")
    .HasApiVersion(1.0);

// Get product by ID
versionedEndpointRouteBuilder
    .MapGet("api/v{version:apiVersion}/products/{id}", async (int id, IMediator mediator) =>
    {
        var query = new GetProductByIdQuery(id);
        var product = await mediator.Send(query);
        
        if (product == null)
        {
            return Results.NotFound();
        }
        
        return Results.Ok(product);
    })
    .WithName("GetProductById")
    .HasApiVersion(1.0);

// Create product
versionedEndpointRouteBuilder
    .MapPost("api/v{version:apiVersion}/products", async (CreateProductCommand command, IMediator mediator) =>
    {
        var product = await mediator.Send(command);
        return Results.Ok(product);
    })
    .WithName("CreateProduct")
    .HasApiVersion(1.0);
// ========================================

// CATEGORIES ENDPOINTS
// ========================================

// Get all categories
versionedEndpointRouteBuilder
    .MapGet("api/v{version:apiVersion}/categories", async (IMediator mediator) =>
    {
        var query = new GetAllCategoriesQuery();
        return await mediator.Send(query);
    })
    .WithName("GetAllCategories")
    .HasApiVersion(1.0);

// Get category by ID
versionedEndpointRouteBuilder
    .MapGet("api/v{version:apiVersion}/categories/{id}", async (int id, IMediator mediator) =>
    {
        var query = new GetCategoryByIdQuery(id);
        var category = await mediator.Send(query);

        if (category == null)
        {
            return Results.NotFound();
        }

        return Results.Ok(category);
    })
    .WithName("GetCategoryById")
    .HasApiVersion(1.0);

// Create category
versionedEndpointRouteBuilder
    .MapPost("api/v{version:apiVersion}/categories", async (CreateCategoryCommand command, IMediator mediator) =>
    {
        var category = await mediator.Send(command);
        return Results.Ok(category);
    })
    .WithName("CreateCategory")
    .HasApiVersion(1.0);

// ========================================

// AUTHENTICATION ENDPOINTS
// ========================================

// Login - authenticate and get JWT token
versionedEndpointRouteBuilder
    .MapPost("api/v{version:apiVersion}/auth/login", async (LoginRequest request, IAuthService authService) =>
    {
        var authResponse = await authService.LoginAsync(request);

        if (authResponse == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(authResponse);
    })
    .WithName("Login")
    .HasApiVersion(1.0);

// Get user profile -- I am not sending the request with the access token in the header 
// requires further implementation to extract user info from the token 
versionedEndpointRouteBuilder
    .MapGet("api/v{version:apiVersion}/auth/profile", async (IAuthService authService) =>
    {
        var profile = await authService.GetProfileAsync();

        if (profile == null)
        {
            return Results.Unauthorized();
        }

        return Results.Ok(profile);
    })
    .WithName("GetProfile")
    .HasApiVersion(1.0);

// ========================================


app.Run();