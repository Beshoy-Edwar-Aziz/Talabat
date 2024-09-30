using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Talabat.Core.Entities.Identity;

namespace Talabat.APIs.Extensions
{
    public static class FindEmailWithUserAddressAsync
    {
        public static async Task<AppUser> GetCurrentUserWithAddress(this UserManager<AppUser> userManager,ClaimsPrincipal User)
        {
            var Email = User.FindFirstValue(ClaimTypes.Email);
            var Result=await userManager.Users.Where(u=>u.Email==Email).Include(u=>u.Address).FirstOrDefaultAsync();
            return Result;
        }
    }
}
