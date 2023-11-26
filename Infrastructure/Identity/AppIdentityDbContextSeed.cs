using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUsersAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Test",
                    Email = "test@test.com",
                    UserName = "test@test.com",
                    Address = new Address 
                    {
                        FirstName = "Test",
                        LastName = "Test",
                        Street = "384 Gauvin Rd",
                        City = "Dieppe",
                        State = "New Brunswick",
                        ZipCode = "E1A 7Y2"
                    }
                };

                await userManager.CreateAsync(user, "Temp!234");
            }
        }
    }
}