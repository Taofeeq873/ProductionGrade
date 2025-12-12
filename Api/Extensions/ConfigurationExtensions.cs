using Api.Middleware;

namespace Api.Extensions;

public static class ConfigurationExtensions
{
    public static void ConfigureCors(this IApplicationBuilder app)
    {
        app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
    }

    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
    }

    public static void ConfigureSwagger(this IApplicationBuilder app, IConfiguration configuration)
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "ProductGrade API");
            options.OAuthClientId(configuration.GetValue<string>("Jwt:Audience"));
            options.OAuthUsePkce();
            options.EnablePersistAuthorization();
        });
    }
}