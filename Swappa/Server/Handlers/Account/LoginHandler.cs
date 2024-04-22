using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Swappa.Data.Contracts;
using Swappa.Entities.Enums;
using Swappa.Entities.Models;
using Swappa.Entities.Responses;
using Swappa.Server.Commands.Account;
using Swappa.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Swappa.Server.Handlers.Account
{
    public class LoginHandler : IRequestHandler<LoginCommand, ResponseModel<TokenDto>>
    {
        private readonly ILogger<LoginHandler> logger;
        private readonly IRepositoryManager repository;
        private readonly UserManager<AppUser> userManager;
        private readonly SignInManager<AppUser> signInManager;
        private readonly ApiResponseDto response;
        private readonly IConfiguration configuration;
        private AppUser? _user;

        public LoginHandler(
            ILogger<LoginHandler> logger, 
            IRepositoryManager repository,
            UserManager<AppUser> userManager,
            SignInManager<AppUser> signInManager,
            ApiResponseDto response,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.repository = repository;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.response = response;
            this.configuration = configuration;
        }

        public async Task<ResponseModel<TokenDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {

            var result = await Validate(request);
            if (result.IsSuccessful)
                return response.Process<TokenDto>(new ApiOkResponse<TokenDto>(await CreateToken(true)));

            return result;
        }

        private async Task<ResponseModel<TokenDto>> Validate(LoginCommand request)
        {
            _user = await userManager.FindByNameAsync(request.Email);
            if (_user == null)
                return response
                    .Process<TokenDto>(new NotFoundResponse($"No user found with this email: {request.Email}"));

            var signinResult = await signInManager.PasswordSignInAsync(_user, request.Password, isPersistent: true, false);
            if (_user.Status != Status.Inactive && signinResult.Succeeded && _user.EmailConfirmed)
            {
                _user.LastLogin = DateTime.Now;
                await userManager.UpdateAsync(_user);
                return response.Process<TokenDto>(new ApiOkResponse<TokenDto>(null!));
            }
            else
            {
                logger.LogWarning($"{nameof(LoginHandler)}: Authentication failed. Wrong password or account not activated yet.");
                return HandleLoginError(_user);
            }
        }

        private async Task<TokenDto> CreateToken(bool populateExp)
        {
            var signingCredentials = GetSigningCredentials();

            var claims = await GetClaims();

            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var refreshToken = GenerateRefreshToken();
            //user.RefreshToken = refreshToken; await userManager.UpdateAsync(user);

            //if (populateExp)
            //    _user.RefreshTokenExpiryTime = DateTime.Now.AddDays(1);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new TokenDto(accessToken, refreshToken);
        }

        private SigningCredentials GetSigningCredentials()
        {
            var setting = configuration.GetSection("JwtSettings");
            var key = Encoding.UTF8.GetBytes(setting["Key"]!);
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim> { new Claim(ClaimTypes.Name, _user?.UserName!), new Claim(ClaimTypes.NameIdentifier.ToString(), _user?.Id.ToString()!) };
            var roles = await userManager.GetRolesAsync(_user!);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken
            (
            issuer: jwtSettings["ValidIssuer"],
            claims: claims,
            expires: DateTime.UtcNow.AddHours(Convert.ToDouble(jwtSettings["Expires"])),
            signingCredentials: signingCredentials
            );
            return tokenOptions;
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ResponseModel<TokenDto> HandleLoginError(AppUser user)
        {
            if (!user.EmailConfirmed)
                return response
                    .Process<TokenDto>(new BadRequestResponse("Email not confirmed. Please confirm your account before attempting to login"));

            else if (user.Status == Status.Inactive && !user.IsDeprecated)
                return response
                    .Process<TokenDto>(new BadRequestResponse("Access denied. Account not deactivated. Please submit a support ticket to reactivate your account."));

            else if (user.IsDeprecated && user.Status != Status.Inactive)
                return response
                    .Process<TokenDto>(new BadRequestResponse("Account deactivated. Reactivation link has been sent to your email address."));
            else
                return response.Process<TokenDto>(new BadRequestResponse("Wrong email or password."));
        }
    }
}