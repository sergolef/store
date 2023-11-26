using API.Errors;
using API.Extensions;
using API.Middleware;
using Core.Entities.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var serverVersion = new MySqlServerVersion(new Version(8, 0, 33));

builder.Services.AddAplicationServices(builder.Configuration); 
builder.Services.AddIdentityServices(builder.Configuration);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


//configure service for collecting validation errors in one enumerable string list
builder.Services.Configure<ApiBehaviorOptions>(options => {
    options.InvalidModelStateResponseFactory = actionContext => {
        
        var errors = actionContext.ModelState
            .Where(e => e.Value.Errors.Count > 0)
            .SelectMany(x => x.Value.Errors)
            .Select(x => x.ErrorMessage).ToArray();

            var errorResponse = new ApiValidationsErrorResponce
            {
                Errors = errors
            };

            return new BadRequestObjectResult(errorResponse);
    };
});


var app = builder.Build();

app.UseMiddleware<ExceptionMiddelware>();

// Configure the HTTP request pipeline.
app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
//app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();


app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<StoreContext>();
//var identityContext = services.GetRequiredService<AppIdentityDbContext>();
//var userManager = services.GetRequiredService<UserManager<AppUser>>();

var loger = services.GetRequiredService<ILogger<Program>>();

try
{
    await context.Database.MigrateAsync();
    //seeding default user for login
   // await identityContext.Database.MigrateAsync();
    await StoreContextSeed.SeedAsync(context);
   /// await AppIdentityDbContextSeed.SeedUsersAsync(userManager);
    
}
catch (Exception ex)
{
    loger.LogError(ex, "An error ocured on running migrations");
    throw;
}
app.Run();
