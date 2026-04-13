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
app.Run();