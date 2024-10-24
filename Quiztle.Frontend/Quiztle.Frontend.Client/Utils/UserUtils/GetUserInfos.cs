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
    }
}
