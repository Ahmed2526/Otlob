using DAL.Dto_s;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Otlob_API.Controllers
{

    public class AccountController : BaseApiController
    {
        public AccountController()
        {
            
        }

        [HttpPost("login")]
        public ActionResult Login(LoginDto loginCredentials)
        {

            return Ok();
        }
    }
}
