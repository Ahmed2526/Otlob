using AutoMapper;
using BLL.IService;
using DAL.Consts;
using DAL.Dto_s;
using DAL.Entities.Identity;
using DAL.Identity;
using DAL.Identity.Migrations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Dynamic.Core.Tokenizer;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BLL.Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly AppIdentityDbContext _identityDbContext;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly JWT _jwt;

        public UserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, ILoggerManager logger, IMapper mapper, IOptions<JWT> jwt, AppIdentityDbContext identityDbContext)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _logger = logger;
            _mapper = mapper;
            _jwt = jwt.Value;
            _identityDbContext = identityDbContext;
        }

        public async Task<CommonApiResponse<UserDto>> Register(RegisterDto RegisterCredentials)
        {
            try
            {
                var errors = new List<string>();

                var checkexist = await _userManager.FindByEmailAsync(RegisterCredentials.Email);
                if (checkexist is not null)
                {
                    errors.Add(Errors.EmailExist);
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, errors);
                }

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
                    foreach (var error in result.Errors)
                    {
                        errors.Add(error.Description);
                    }
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, errors);
                }

                var jwtSecurityToken = await CreateJwtToken(user);
                var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                var RefreshToken = GenerateRefreshToken();
                RefreshToken.ApplicationUserId = user.Id;

                await _identityDbContext.AddAsync(RefreshToken);
                await _identityDbContext.SaveChangesAsync();

                return new CommonApiResponse<UserDto>(HttpStatusCode.OK, new UserDto() { Email = user.Email, Token = token, RefreshToken = RefreshToken.Token, ExpiresOn = RefreshToken.ExpriesOn });
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

                    var ValidRefreshTkExist = _identityDbContext.RefreshTokens.FirstOrDefault(u => u.ApplicationUserId == user.Id && u.ExpriesOn > DateTime.Now && u.RevokedOn == null);
                    if (ValidRefreshTkExist is not null)
                    {
                        userdto.RefreshToken = ValidRefreshTkExist.Token;
                        userdto.ExpiresOn = ValidRefreshTkExist.ExpriesOn;
                    }
                    else
                    {
                        var RefreshToken = GenerateRefreshToken();
                        RefreshToken.ApplicationUserId = user.Id;

                        await _identityDbContext.AddAsync(RefreshToken);
                        await _identityDbContext.SaveChangesAsync();

                        userdto.RefreshToken = RefreshToken.Token;
                        userdto.ExpiresOn = RefreshToken.ExpriesOn;
                    }


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

        public async Task<CommonApiResponse<UserAccountDto>> GetCurrentUser(ClaimsPrincipal userClaims)
        {
            try
            {
                var email = userClaims.FindFirstValue(ClaimTypes.Email);

                var user = await _userManager.Users.Include(a => a.Address)
                    .SingleOrDefaultAsync(e => e.Email == email);

                var userDto = _mapper.Map<UserAccountDto>(user);

                return new CommonApiResponse<UserAccountDto>(HttpStatusCode.OK, userDto);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(GetCurrentUser)} method {ex}");
                throw;
            }

        }

        public async Task<CommonApiResponse<UserDto>> RenewToken(string expiredToken, string refreshToken)
        {
            try
            {
                var checkValidateOldToken = GetPrincipalFromExpiredToken(expiredToken);

                if (checkValidateOldToken is null)
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, new List<string> { Errors.InvalidToken });

                var user = await _userManager.Users
                      .SingleOrDefaultAsync(e => e.RefreshTokens
                      .Any(e => e.Token == refreshToken));

                var Email = checkValidateOldToken.FindFirstValue(ClaimTypes.Email);

                if (user is null || user.Email != Email)
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, new List<string> { Errors.InvalidToken });

                var oldRefreshToken = _identityDbContext.RefreshTokens
                    .SingleOrDefault(e => e.Token == refreshToken && e.RevokedOn == null && e.ExpriesOn > DateTime.Now);

                if (oldRefreshToken is null)
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, new List<string> { Errors.InvalidRefreshToken });

                //Create New Token
                var jwtSecurityToken = await CreateJwtToken(user);
                var NewToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

                // Create New RefreshToken
                oldRefreshToken.RevokedOn = DateTime.Now;
                oldRefreshToken.ExpriesOn = DateTime.Now;

                var NewRefreshToken = GenerateRefreshToken();
                NewRefreshToken.ApplicationUserId = user.Id;

                await _identityDbContext.AddAsync(NewRefreshToken);
                _identityDbContext.RefreshTokens.Update(oldRefreshToken);
                await _identityDbContext.SaveChangesAsync();

                var userDto = new UserDto()
                {
                    Email = user.Email,
                    Token = NewToken,
                    RefreshToken = NewRefreshToken.Token,
                    ExpiresOn = NewRefreshToken.ExpriesOn
                };

                return new CommonApiResponse<UserDto>(HttpStatusCode.OK, userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(RenewToken)} method {ex}");
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
                expires: DateTime.Now.AddDays(_jwt.TokenDurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private RefreshToken GenerateRefreshToken()
        {
            var randomNumber = new byte[32];

            using var generator = new RNGCryptoServiceProvider();

            generator.GetBytes(randomNumber);

            return new RefreshToken
            {
                Token = Convert.ToBase64String(randomNumber),
                CreatedOn = DateTime.Now,
                ExpriesOn = DateTime.Now.AddDays(_jwt.RefreshTokenDurationInDays)
            };
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            try
            {
                var tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
                    ValidateIssuer = false,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key)),
                    ValidateLifetime = false //here we are saying that we don't care about the token's expiration date
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                SecurityToken securityToken;

                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
                var jwtSecurityToken = securityToken as JwtSecurityToken;
                if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                    return null;

                return principal;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(GetPrincipalFromExpiredToken)} method {ex}");
                return null;
            }
        }

        public async Task<CommonApiResponse<UserDto>> RevokeToken(string refreshToken)
        {
            try
            {
                var token = await _identityDbContext.RefreshTokens.FirstOrDefaultAsync(e => e.Token == refreshToken);

                if (token is null)
                    return new CommonApiResponse<UserDto>(HttpStatusCode.BadRequest, null, new List<string> { Errors.InvalidRefreshToken });

                token.RevokedOn = DateTime.Now;
                token.ExpriesOn = DateTime.Now;

                _identityDbContext.Update(token);
                _identityDbContext.SaveChanges();

                return new CommonApiResponse<UserDto>(HttpStatusCode.OK, null);
            }

            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong in the" +
                   $" {nameof(UserService)} in {nameof(RevokeToken)} method {ex}");
                throw;
            }
        }
    }
}
