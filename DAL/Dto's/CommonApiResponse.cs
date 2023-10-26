using System.Net;

namespace DAL.Dto_s
{
    public class CommonApiResponse<T>
    {
        public int StatusCode { get; set; }
        public T? User { get; set; }
        public List<string>? Errors { get; set; }

        public CommonApiResponse(HttpStatusCode statusCode, T user, List<string> errors = null)
        {
            StatusCode = (int)statusCode;
            User = user;
            Errors = errors;
        }


    }
}
