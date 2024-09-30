namespace Talabat.APIs.Errors
{
    public class ApiServerExceptionResponse : ApiResponse
    {
        public string? Details { get; set; }
        public ApiServerExceptionResponse(int statuscode, string? message = null,string? details= null):base(statuscode, message)
        {
            Details = details;
        }
    }
}
