using Microsoft.OpenApi.Models;
using System.Reflection;

namespace Interface
{
    public static class SwaggerDiExtensions
    {
        public static IServiceCollection ConfigureSwagger(this IServiceCollection services, string title, string version = "v1")
        {
            services.AddSwaggerGen(c =>
            {

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);


                //                if (File.Exists(xmlPath))
                //c.IncludeXmlComments(xmlPath);

                c.SwaggerDoc(version, new OpenApiInfo
                {
                    Version = version,
                    Title = title
                });


                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description =
                        "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    },
                });

                //c.OperationFilter<LangHeaderParam>();

            });//AddSwaggerGenNewtonsoftSupport();
            return services;
        }
    }
}