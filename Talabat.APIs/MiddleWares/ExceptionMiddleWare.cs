using System.Net;
using System.Text.Json;
using Talabat.APIs.Errors;

namespace Talabat.APIs.MiddleWares
{
    public class ExceptionMiddleWare
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleWare> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleWare(RequestDelegate next, ILogger<ExceptionMiddleWare> logger, IHostEnvironment env)
        {
            _next = next;
            _logger = logger;
            _env = env;
        }
        public async Task InvokeAsync(HttpContext Context)
        {
            try
            {
                await _next.Invoke(Context);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                Context.Response.ContentType = "application/json";
                Context.Response.StatusCode= (int) HttpStatusCode.InternalServerError;
               
                //if (_env.IsDevelopment())
                //{
                    
                //    var Response = new ApiServerExceptionResponse(500,ex.Message,ex.StackTrace.ToString());
                //}
                //else
                //{
                //    var Response = new ApiServerExceptionResponse(500);
                //}
              var Response=  _env.IsDevelopment() ? new ApiServerExceptionResponse(500, ex.Message, ex.StackTrace.ToString()) : new ApiServerExceptionResponse(500);
                var Options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var Json = JsonSerializer.Serialize(Response, Options);
                await Context.Response.WriteAsync(Json);
            }
            
        }
    }
}
