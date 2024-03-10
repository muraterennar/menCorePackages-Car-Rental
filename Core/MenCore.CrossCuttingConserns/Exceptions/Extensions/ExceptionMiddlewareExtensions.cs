using Microsoft.AspNetCore.Builder;

namespace MenCore.CrossCuttingConserns.Exceptions.Extensions;

public static class ExceptionMiddlewareExtensions
{
    public static void ConfigureCustomExceptionMiddleware (this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }
}