using BLL.IService;
using DAL.Dto_s;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Otlob_API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerCredentials)
        {
            var result = await _userService.Register(registerCredentials);

            if (result.User is null)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.User.RefreshToken, result.User.ExpiresOn);

            return Ok(result);
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginCredentials)
        {
            var result = await _userService.Login(loginCredentials);

            if (result.User is null)
                return Unauthorized(result);

            SetRefreshTokenInCookie(result.User.RefreshToken, result.User.ExpiresOn);

            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult> GetCurrentUser()
        {
            var currentUser = User;
            var result = await _userService.GetCurrentUser(currentUser);

            if (result.User is null)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("RenewToken")]
        public async Task<ActionResult> RenewToken([FromBody] string expiredToken)
        {
            string refreshToken = Request.Cookies["refreshToken"]!;

            var result = await _userService.RenewToken(expiredToken, refreshToken);

            if (result.User is null)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.User.RefreshToken, result.User.ExpiresOn);

            return Ok(result);
        }

        [HttpPost("RevokeToken")]
        public async Task<ActionResult> RevokeToken()
        {
            string refreshToken = Request.Cookies["refreshToken"]!;

            var result = await _userService.RevokeToken(refreshToken);

            if (result.Errors is not null)
                return BadRequest(result);

            Response.Cookies.Delete("refreshToken");
            return Ok(result);
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
    }
}
