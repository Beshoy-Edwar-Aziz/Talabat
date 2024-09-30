using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using Talabat.APIs.Extensions;
using Talabat.APIs.Helpers;
using Talabat.APIs.MiddleWares;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Identity;
using Talabat.Core.Repositiories;
using Talabat.Repository;
using Talabat.Repository.Data;
using Talabat.Repository.Identity;

namespace Talabat.APIs
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            #region Configure Services

            //builder.Services.AddControllers();
            //// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            //builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();
            //builder.Services.AddDbContext<TalabatContext>(options=>options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            //builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
            ////builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
            ////or
            //builder.Services.AddAutoMapper(typeof(MappingProfiles));
            //builder.Services.Configure<ApiBehaviorOptions>(options =>
            //{
            //    options.InvalidModelStateResponseFactory = (actionContext) =>
            //    {
            //        var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
            //                                             .SelectMany(p => p.Value.Errors)
            //                                             .Select(p => p.ErrorMessage)
            //                                             .ToArray();
            //        var ValidationResponse = new ApiValidationResponse()
            //        {
            //            Errors = errors
            //        };
            //        return new BadRequestObjectResult(ValidationResponse);
            //    };
            //});
            builder.Services.serviceExtension(builder.Configuration);
            //builder.Services.AddDbContext<IdentitDbContext>(options =>
            //{
            //    options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection"));
            //});
            //builder.Services.AddAuthentication();
            //builder.Services.AddIdentity<AppUser, IdentityRole>()
            //                 .AddEntityFrameworkStores<IdentitDbContext>();
            builder.Services.IdentityServices(builder.Configuration);
            #endregion

            var app = builder.Build();
            #region UpdateDatabase
            using var Scope = app.Services.CreateScope();
            var Services = Scope.ServiceProvider;
            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            var UserManager = Services.GetRequiredService<UserManager<AppUser>>();
            try
            {
                var DbContext = Services.GetRequiredService<TalabatContext>();
                await DbContext.Database.MigrateAsync();
                await TalabatContextSeed.SeedDataAsync(DbContext);
                var IdentityDbContext = Services.GetRequiredService<IdentitDbContext>();
                await IdentityDbContext.Database.MigrateAsync();
                await IdentitDbContextSeed.SeedDataAsync(UserManager);
            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "Exception Occured During Migration");
            } 
            #endregion

            // Configure the HTTP request pipeline.
            #region Configure
            if (app.Environment.IsDevelopment())
            {
                app.UseMiddleware<ExceptionMiddleWare>();
                app.AppExtension();
            }

            app.UseStatusCodePagesWithReExecute("/Error/{0}");
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers(); 
            #endregion

            app.Run();
        }
    }
}
