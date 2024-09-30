using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Services;
using Talabat.Repository.Identity;
using Talabat.Services;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Talabat.APIs.Extensions
{
    public static class Identity
    {
        public static IServiceCollection IdentityServices (this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddScoped<ITokenService, TokenService>();
            Services.AddIdentity<AppUser, IdentityRole>()
                            .AddEntityFrameworkStores<IdentitDbContext>();
            Services.AddDbContext<IdentitDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
            Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }
            ).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidIssuer = Configuration["JWT:ValidIssuer"],
                    ValidateAudience = true,
                    ValidAudience = Configuration["JWT:ValidAudience"],
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["JWT:Key"]))
                };
            });
           
            return Services;
        }
    }
}
