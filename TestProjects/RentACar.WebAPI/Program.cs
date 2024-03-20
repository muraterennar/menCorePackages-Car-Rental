using MenCore.CrossCuttingConserns.Exceptions.Extensions;
using RentACar.Application;
using RentACar.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();

// --> InMemoryCache
//builder.Services.AddDistributedMemoryCache();

// --> RedisCaches
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["CacheSettings:CacheUri"];
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();

//if (app.Environment.IsProduction())
    app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

