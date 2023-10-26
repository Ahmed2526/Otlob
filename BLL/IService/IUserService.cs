using DAL.Dto_s;
using DAL.Entities.Identity;
using System.Security.Claims;

namespace BLL.IService
{
    public interface IUserService
    {
        Task<CommonApiResponse<UserDto>> Login(LoginDto loginCredentials);
        Task<CommonApiResponse<UserDto>> Register(RegisterDto registerCredentials);
        Task<CommonApiResponse<UserAccountDto>> GetCurrentUser(ClaimsPrincipal userClaims);
        Task<CommonApiResponse<UserDto>> RenewToken(string expiredToken, string refreshToken);
        Task<CommonApiResponse<UserDto>> RevokeToken(string refreshToken);

    }
}
