using System.Text.Json.Serialization;

namespace DAL.Dto_s
{
    public class UserDto
    {
        public string Email { get; set; }
        public string Token { get; set; }

        [JsonIgnore]
        public string RefreshToken { get; set; }
        public DateTime ExpiresOn { get; set; }
    }
}
