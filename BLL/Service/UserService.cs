using AutoMapper;
using BLL.IService;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace BLL.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILoggerManager logger, IMapper mapper, IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt.Value;
        }

        public async Task<CommonApiResponse<UserDto>> Register(RegisterDto RegisterCredentials)
        {
            try
            {
                var user = new ApplicationUser()
                {
                    FirstName = RegisterCredentials.FirstName,
                    LastName = RegisterCredentials.LastName,
                    Email = RegisterCredentials.Email,
                    PhoneNumber = RegisterCredentials.PhoneNumber,
                    UserName = RegisterCredentials.Email
                };

                var address = new Address()
                {
                    Country = RegisterCredentials.Country,
                    City = RegisterCredentials.City,
                    Street = RegisterCredentials.Street,
                    ZipCode = RegisterCredentials.ZipCode
                };

                user.Address = address;
                user.AddressId = address.Id;

                var result = await _userManager.CreateAsync(user, RegisterCredentials.Password);

                if (!result.Succeeded)
                {
                    var errors = new List<string>();
                    foreach (var item in result.Errors)
                    {
                        errors.Add(item.Description);
                    }
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, errors);
                }

                var jwtSecurityToken = await CreateJwtToken(user);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                return new CommonApiResponse<UserDto>(HttpStatusCode.OK, new UserDto() { Email = user.Email, Token = token });
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(Register)} method {ex}");
                throw;
            }

        }

        public async Task<CommonApiResponse<UserDto>> Login(LoginDto loginCredentials)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(loginCredentials.Email);

                var errors = new List<string>();
                errors.Add("Invalid Email or password");

                if (user is null)
                    return new CommonApiResponse<UserDto>(HttpStatusCode.Unauthorized, null, errors);

                var result = await _signInManager.PasswordSignInAsync(user, loginCredentials.Password, false, false);

                if (result.Succeeded)
                {
                    var jwtSecurityToken = await CreateJwtToken(user);
                    var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                    var userdto = new UserDto()
                    {
                        Email = user.Email,
                        Token = token
                    };

                    return new CommonApiResponse<UserDto>(HttpStatusCode.OK, userdto);
                }

                return new CommonApiResponse<UserDto>(HttpStatusCode.Unauthorized, null, errors);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(Login)} method {ex}");
                throw;
            }
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

    }
}
