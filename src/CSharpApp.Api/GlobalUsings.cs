// Global using directives

global using CSharpApp.Core.Interfaces;
global using CSharpApp.Infrastructure.Configuration;
global using Serilog;
global using MediatR;
global using CSharpApp.Application.Products.Commands.CreateProduct;  
global using CSharpApp.Application.Products.Queries.GetAllProducts;  
global using CSharpApp.Application.Products.Queries.GetProductById;
global using CSharpApp.Application.Categories.Commands.CreateCategory;  
global using CSharpApp.Application.Categories.Queries.GetAllCategories; 
global using CSharpApp.Application.Categories.Queries.GetCategoryById;
global using CSharpApp.Core.Dtos.Requests;    
global using CSharpApp.Core.Dtos.Responses;
global using CSharpApp.Api.Middleware;