using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Talabat.Core.Entities.Identity;

namespace Talabat.Repository.Identity
{
    public static class IdentitDbContextSeed
    {
        public static async Task SeedDataAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var User = new AppUser()
                {
                    DisplayName = "BOsh",
                    Email = "beshoyedwar8@gmail.com",
                    UserName = "BeshoyEdwar",
                    PhoneNumber = "01201493556"
                };
                await userManager.CreateAsync(User,"Pa$$w0rd");
            }
        }
    }
}
