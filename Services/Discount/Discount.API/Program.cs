using System.Reflection;
using Discount.API.Services;
using Discount.Application.Handlers;
using Discount.Core.Repositories;
using Discount.Infrastructure.Extensions;
using Discount.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);


//Register AutoMapper
builder.Services.AddAutoMapper(typeof(Program).Assembly);
var assemblies = new Assembly[]
{
    Assembly.GetExecutingAssembly(),
    typeof(CreateDiscountHandler).Assembly,
};

//Register Mediatr
builder.Services.AddMediatR(c => c.RegisterServicesFromAssemblies(assemblies));
builder.Services.AddScoped<IDiscountRepository, DiscountRepository>();
builder.Services.AddGrpc();

var app = builder.Build();

//Migrate Database
app.MigrateDatabase<Program>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseRouting();

app.UseEndpoints(e =>
{
    e.MapGrpcService<DiscountService>();
    e.MapGet("/", async context =>
    {
        await context.Response.WriteAsync("Communication with gRPC endpoints must be made through a gRPC service.");
    }); 
});

app.Run();