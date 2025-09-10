using Microsoft.AspNetCore.Builder;

namespace ProductCatalog.Api.Middlewares
{
    public static class ExceptionHandlingExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
            => app.UseMiddleware<ExceptionHandlingMiddleware>();
    }
}
