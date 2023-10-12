using System.Net;

namespace Otlob_API.ErrorModel
{
    public class ApiValidationErrorResponse : ApiResponse
    {
        public IEnumerable<string> Errors { get; set; }
        public ApiValidationErrorResponse() : base((int)HttpStatusCode.BadRequest)
        {

        }
    }
}
