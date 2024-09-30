namespace Talabat.APIs.Errors
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public ApiResponse(int statuscode, string? message = null) 
        {
            StatusCode = statuscode;
            Message = message ?? ApplyDefaultMessage(statuscode);
        }
        private string? ApplyDefaultMessage(int statuscode)
        {
            return statuscode switch
            {
                400 => "Bad Request",
                401 => "UnAuthorized",
                404 => "Resource Not Found",
                500 => "Internal Server Error",
                _ => null
            };
        }
    }
}
