using System.Reflection;
using Catalog.Application.Handlers;
using Catalog.Core.Repositories;
using Catalog.Infrastructure.Data;
using Catalog.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

//Add API Versioning
builder.Services.AddApiVersioning(x =>
{
    x.ReportApiVersions = true;
    x.AssumeDefaultVersionWhenUnspecified = true;
    x.DefaultApiVersion = new ApiVersion(1, 0);
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",new OpenApiInfo
    {
        Title = "Catalog API",
        Version = "v1"
    });
});

//Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(GetAllBrandsHandler).Assembly,
};

//Register Mediatr
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies));

//Register Application Services
builder.Services.AddScoped<ICatalogContext, CatalogContext>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IBrandRepository, ProductRepository>();
builder.Services.AddScoped<ITypesRepository, ProductRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();