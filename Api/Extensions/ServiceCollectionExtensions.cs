
using System.Text.Json;
using System.Text.Json.Serialization;
using Api.Filters;
using Application.Extensions;
using FluentValidation;
using Mapster;
using MapsterMapper;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.OpenApi.Models;

namespace Api.Extensions;

public static class ServiceCollectionExtensions
{
    public static void ConfigureApiVersioning(this IServiceCollection services)
    {
        services.AddApiVersioning(config =>
        {
            config.DefaultApiVersion = new ApiVersion(1, 0);
            config.AssumeDefaultVersionWhenUnspecified = true;
            config.ReportApiVersions = true;
        });
    }

    public static void ConfigureMvc(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddControllers(options =>
            {
                options.OutputFormatters.RemoveType<StringOutputFormatter>();
                options.Filters.Add<ValidationFilter>();
                options.ModelValidatorProviders.Clear();
            })
            .ConfigureApiBehaviorOptions(options => options.SuppressModelStateInvalidFilter = true)
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
            });
    }

    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "API",
                    Version = "v1",
                    Contact = new OpenApiContact
                    {
                        Name = "E-mail",
                        Email = ""
                    }
                });

            // Define the OAuth2 scheme that uses the JWT bearer token.
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description =
                    "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer", // Must be lowercase.
                BearerFormat = "JWT"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    },
                    new List<string>()
                }
            });
        });
        services.AddFluentValidationRulesToSwagger();
    }

    public static IServiceCollection AddMapster(this IServiceCollection services)
    {
        var config = TypeAdapterConfig.GlobalSettings;
        config.Default.EnumMappingStrategy(EnumMappingStrategy.ByName);
        services.AddSingleton(config);
        services.AddScoped<IMapper, ServiceMapper>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection serviceCollection)
    {
        ValidatorOptions.Global.DisplayNameResolver = (_, member, _) => member?.Name?.ToCamelCase();

        ValidatorOptions.Global.PropertyNameResolver = (_, member, _) => member?.Name?.ToCamelCase();


        return serviceCollection;
           
    }
}
