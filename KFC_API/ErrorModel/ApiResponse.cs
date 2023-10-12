using System.Text.Json;

namespace Otlob_API.ErrorModel
{
    public class ApiResponse
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public override string ToString()
        {
            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
            return JsonSerializer.Serialize(this, serializeOptions);
        }

        private string GetDefaultMessageForStatusCode(int statusCode)
           => statusCode switch
           {
               400 => "Bad Request",
               401 => "You are not authorized!",
               404 => "Resource was not found",
               500 => "Internal Server Error",
               _ => string.Empty
           };

    }
}
