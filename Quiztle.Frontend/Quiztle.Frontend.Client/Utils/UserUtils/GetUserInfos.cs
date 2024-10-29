using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Quiztle.Frontend.Client.Utils
{
    public class GetUserInfos
    {
        private readonly AuthenticationStateProvider _authenticationStateProvider;

        public GetUserInfos(AuthenticationStateProvider authenticationStateProvider)
        {
            _authenticationStateProvider = authenticationStateProvider;
        }

        public async Task<string> GetUserId()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
                return userIdClaim?.Value!;
            }

            return "";
        }

        public async Task<ClaimsPrincipal> GetUserTotalInfo()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User;
        }

        public async Task<Claim[]> GetUserClaims()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                return user.Claims.ToArray();
            }

            return [];
        }

        public async Task<string> GetUserEmail()
        {
            var emailClaim = await GetClaimValue(ClaimTypes.Email);
            return emailClaim ?? "";
        }

        public async Task<string> GetUserName()
        {
            var nameClaim = await GetClaimValue(ClaimTypes.Name);
            return !string.IsNullOrEmpty(nameClaim) ? nameClaim : await GetUserEmail();
        }

        public async Task<Dictionary<string, string>> GetUserNameAndEmail()
        {
            var claims = new Dictionary<string, string>
            {
                ["Name"] = await GetClaimValue(ClaimTypes.Name) ?? "",
                ["Email"] = await GetClaimValue(ClaimTypes.Email) ?? ""
            };

            return claims;
        }


        private async Task<string?> GetClaimValue(string claimType)
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity?.IsAuthenticated == true)
            {
                return user.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
            }

            return null;
        }

        public async Task<bool> IsAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

    }
}