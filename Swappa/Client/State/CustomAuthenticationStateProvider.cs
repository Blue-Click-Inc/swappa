using Microsoft.AspNetCore.Components.Authorization;
using Swappa.Shared.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Swappa.Client.State
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ClaimsPrincipal anonymous = new(new ClaimsIdentity());

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                if (string.IsNullOrEmpty(Constants.JwtToken))
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var userClaims = DecryptToken(Constants.JwtToken);
                if(userClaims == null)
                    return await Task.FromResult(new AuthenticationState(anonymous));

                var claimPrincipal = SetClaimPrincipal(userClaims);
                return await Task.FromResult(new AuthenticationState(claimPrincipal));
            }
            catch (Exception)
            {
                return await Task.FromResult(new AuthenticationState(anonymous));
            }
        }

        private static ClaimsPrincipal SetClaimPrincipal(CustomUserClaims claims)
        {
            if (claims.UserName is null || !claims.Roles.Any() || claims.Id is null) 
                return new ClaimsPrincipal();

            var claimList = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, claims.Id),
                new(ClaimTypes.Name, claims.UserName)
            };

            foreach (var role in claims.Roles)
            {
                claimList.Add(new(ClaimTypes.Role, role));
            }


            var claimPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claimList, "JwtAuth"));

            return claimPrincipal;
        }

        private static CustomUserClaims DecryptToken(string accessToken)
        {
            if(string.IsNullOrEmpty(accessToken))
                return new CustomUserClaims();

            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(accessToken);

            var userName = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.Name);
            var id = token.Claims.FirstOrDefault(_ => _.Type == ClaimTypes.NameIdentifier);
            var roles = token.Claims.Where(_ => _.Type == ClaimTypes.Role).Select(_ => _.Value).ToList();

            return new CustomUserClaims(id!.Value, userName!.Value, roles);
        }

        public void UpdateAuthenticationState(string accessToken)
        {
            var claimsPrincipal = new ClaimsPrincipal();
            if (!string.IsNullOrEmpty(accessToken))
            {
                Constants.JwtToken = accessToken;
                var claims = DecryptToken(accessToken);
                claimsPrincipal = SetClaimPrincipal(claims);
            }
            else
            {
                Constants.JwtToken = String.Empty;
            }

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}
