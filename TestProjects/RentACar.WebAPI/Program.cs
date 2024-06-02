using MenCore.CrossCuttingConserns.Exceptions.Extensions;
using MenCore.Mailing;
using MenCore.Security;
using MenCore.Security.Encryption;
using MenCore.Security.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RentACar.Application;
using RentACar.Infrastructure;
using RentACar.Persistence;
using RentACar.WebAPI;

var builder = WebApplication.CreateBuilder(args);
// Add services
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureService();
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddSecurityService();
builder.Services.AddMailingService();


var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = tokenOptions.Issuer,
            ValidAudience = tokenOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
        };
    });
// --> InMemoryCache
//builder.Services.AddDistributedMemoryCache();

// --> RedisCaches
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration["CacheSettings:CacheUri"];
});

// CORS Politikalarının belirlendiği alan
var originUrls = builder.Configuration.GetSection("WebAPIConfiguration").Get<WebAPIConfiguration>().AllowedOrigins;
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
        builder.WithOrigins(originUrls) // Üretim sunucusunun URL'sini ekleyin
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition(
        name: "Bearer",
        securityScheme: new OpenApiSecurityScheme
        {
            Name = "Authorization",
            Type = SecuritySchemeType.ApiKey,
            Scheme = "Bearer",
            BearerFormat = "JWT",
            In = ParameterLocation.Header,
            Description =
                "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345.54321\""
        });
    opt.AddSecurityRequirement(
        new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                new string[] { }
            }
        }
    );
});

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

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();