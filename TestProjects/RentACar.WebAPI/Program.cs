using MenCore.CrossCuttingConserns.Exceptions.Extensions;
using MenCore.Security;
using RentACar.Application;
using RentACar.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddApplicationServices();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddSecurityService();

// --> InMemoryCache
//builder.Services.AddDistributedMemoryCache();

// --> RedisCaches
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["CacheSettings:CacheUri"];
});

// CORS Politikalarının belirlendiği alan
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevelopmentPolicy", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyHeader()
               .AllowAnyMethod();
    });

    options.AddPolicy("ProductionPolicy", builder =>
    {
        builder.WithOrigins("https://example.com") // Üretim sunucusunun URL'sini ekleyin
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

if (app.Environment.IsProduction())
    app.ConfigureCustomExceptionMiddleware();

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
    app.UseCors("DevelopmentPolicy");
else
    app.UseCors("ProductionPolicy");

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

