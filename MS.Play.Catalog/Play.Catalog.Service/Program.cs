using MassTransit;
using Microsoft.OpenApi.Models;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Common.MassTransit;
using Play.Common.MongoDB;
using Play.Common.Settings;


var builder = WebApplication.CreateBuilder(args);

var serviceSettings = builder.Configuration.GetSection(nameof(ServiceSettings)).Get<ServiceSettings>(); //ServiceSettings

builder.Services.AddMongo()
    .AddMongoRepository<Item>("items")
    .AddMassTransitWithRabitMq();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "items", Version = "v1" });
    c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First()); //This line
});
builder.Services.AddControllers(options =>    // Disable Suppress "Async" Suffix 
{
    options.SuppressAsyncSuffixInActionNames = false;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
