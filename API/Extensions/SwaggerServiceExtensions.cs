using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.OpenApi.Models;

namespace API.Extensions
{
    public static class SwaggerServiceExtensions
    {
        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                var securitySchema = new OpenApiSecurityScheme
                {
                    Description = "JWT Auth Bearer Schema",
                    Name = "Authorisation",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    }
                };

                c.AddSecurityDefinition("Bearer", securitySchema);

                var securityRequirements = new OpenApiSecurityRequirement
                {
                    {
                        securitySchema, new[] {"Bearer"}
                    }
                };

                c.AddSecurityRequirement(securityRequirements);

            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerDomumentstion(this IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();

            return app;
        }
    }
}