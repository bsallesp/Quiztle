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

        public async Task<string> GetUserNameOrEmail()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            var user = authState.User;

            if (user.Identity != null && user.Identity.IsAuthenticated)
            {
                // Tentando obter o nome
                var nameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
                if (!string.IsNullOrEmpty(nameClaim?.Value))
                {
                    return nameClaim.Value;
                }

                // Se o nome estiver vazio, tentar obter o email
                var emailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                return emailClaim?.Value ?? "";
            }

            return "";
        }

        public async Task<bool> IsAuthenticated()
        {
            var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }
    }
}
