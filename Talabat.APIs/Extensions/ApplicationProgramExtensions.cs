using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Helpers;
using Talabat.Core.Repositiories;
using Talabat.Repository.Data;
using Talabat.Repository;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.Errors;
using StackExchange.Redis;
using Talabat.Core;
using Talabat.Core.Services;
using Talabat.Services;

namespace Talabat.APIs.Extensions
{
    public static class ApplicationProgramExtensions
    {

        public static IServiceCollection serviceExtension(this IServiceCollection Services, IConfiguration configuration)
        {
           
            Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            Services.AddEndpointsApiExplorer();
            Services.AddSwaggerGen();
            Services.AddDbContext<TalabatContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            //builder.Services.AddAutoMapper(M=>M.AddProfile(new MappingProfiles()));
            //or
            Services.AddAutoMapper(typeof(MappingProfiles));
            Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState.Where(P => P.Value.Errors.Count > 0)
                                                         .SelectMany(p => p.Value.Errors)
                                                         .Select(p => p.ErrorMessage)
                                                         .ToArray();
                    var ValidationResponse = new ApiValidationResponse()
                    {
                        Errors = errors
                    };
                    return new BadRequestObjectResult(ValidationResponse);
                };
            });
            Services.AddSingleton<IConnectionMultiplexer>(options =>
            {
                var Connection =configuration.GetConnectionString("Redis");
                return ConnectionMultiplexer.Connect(Connection);
            });
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IOrderService, OrderService>();
            Services.AddScoped<IPaymentService, PaymentService>();
            Services.AddSingleton<IResponseCacheService, ResponseCacheService>();
            
            return Services;
        }
        public static WebApplication AppExtension(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            return app;
        }
    }
}
