using System;
using API.Errors;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

// using API.Helpers;
// using API.Intrefaces;
// using API.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql;
using StackExchange.Redis;

namespace API.Extensions
{
    public static class ApplicationServicesExtension
    {
        public static IServiceCollection AddAplicationServices(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config.GetConnectionString("DefaultConnection");

            //services.Configure<CloudinaryConfig>(config.GetSection("CloudinarySettings"));

            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));

            //add repositories
            
            services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddScoped<IBasketRepository, BasketRepository>();

            
            
            
            // services.AddScoped<ILikeRepository, LikeRepository>();
            // services.AddScoped<IMessageRepository, MessageRepository>();

            //mysql connection
            var connectionStringBuilder = new NpgsqlConnectionStringBuilder(connectionString);
            var searchPaths = connectionStringBuilder.SearchPath?.Split(',');

            services.AddDbContext<StoreContext>(options =>
            {
                options.UseNpgsql(connectionString, o =>
                {
                    //configure(o);

                    if (searchPaths is {Length: > 0})
                    {
                        var mainSchema = searchPaths[0];
                        o.MigrationsHistoryTable(HistoryRepository.DefaultTableName, mainSchema);
                    }
                });
            });

            //add redis connection
            services.AddSingleton<IConnectionMultiplexer>( c => {
                var options = ConfigurationOptions.Parse(config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(options);
            });


            // services.AddSpaStaticFiles(configuration =>
            // {
            //     configuration.RootPath = "ClientApp/dist";
            // });

            // //add services
            // services.AddScoped<ITokenService, TokenService>();
            // services.AddScoped<IPhotoService, PhotoService>();
            // services.AddScoped<LogUserAcivity>();


            // //add mapers
            // services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            //add CORS support
            services.AddCors( opt => {
                opt.AddPolicy("CorsPolicy", policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
            return services;
        }
    }
}

