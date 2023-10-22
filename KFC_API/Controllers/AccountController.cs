using BLL.IService;
using DAL.Dto_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Otlob_API.ErrorModel;
using System.Net;

namespace Otlob_API.Controllers
{

    public class AccountController : BaseApiController
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginDto loginCredentials)
        {
            var result = await _userService.Login(loginCredentials);

            if (result.User is null)
                return Unauthorized(result);

            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register(RegisterDto registerCredentials)
        {
            var result = await _userService.Register(registerCredentials);

            if (result.User is null)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
