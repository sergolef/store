using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;


namespace API.Extensions
{
    public static class IdentityServiceExtensions
    {
 

        public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config )
        {
            var connectionString = config.GetConnectionString("IdentityConnection");

            //var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));

            services.AddDbContext<AppIdentityDbContext>(options =>
            {
                options.UseNpgsql(connectionString);
            });

            services.AddIdentityCore<AppUser>(options => {
                //add identity options here
            })
            .AddEntityFrameworkStores<AppIdentityDbContext>()
            .AddSignInManager<SignInManager<AppUser>>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"])),
                        ValidIssuer = config["Token:Issuer"],
                        ValidateIssuer = true,
                        ValidateAudience = false
                   };
                });




            services.AddAuthorization();

            return services;
        }
    }
}