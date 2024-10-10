using Quiztle.CoreBusiness;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Quiztle.Blazor.Client.Authentication.Core
{
    public class DefaultAuthenticationStateProvider : AuthenticationStateProvider
    {
        public User? CurrentUser { get; set; }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();

            if (CurrentUser != null && !string.IsNullOrWhiteSpace(CurrentUser.Email) && !string.IsNullOrWhiteSpace(CurrentUser.Password))
            {
                var claims = new List<Claim>
                {
                    new(ClaimTypes.Name, CurrentUser.Name),
                    new(ClaimTypes.Role, CurrentUser.Role),
                    new(ClaimTypes.Email, CurrentUser.Email)
                };

                var identity = new ClaimsIdentity(claims, "CustomAuthType");

                principal = new ClaimsPrincipal(identity);
            }

            return Task.FromResult(new AuthenticationState(principal));
        }

        public void LogoutAsync()
        {
            CurrentUser = null;

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }

        public void NotifyStateChanged(ClaimsPrincipal principal)
        {
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public void SetCurrentUser(User user)
        {
            CurrentUser = user;

            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, CurrentUser.Name),
                new(ClaimTypes.Role, CurrentUser.Role),
                new(ClaimTypes.Email, CurrentUser.Email)
            };

            var identity = new ClaimsIdentity(claims, "CustomAuthType");
            var principal = new ClaimsPrincipal(identity);

            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public bool IsUserAuthenticated()
        {
            return CurrentUser != null && !string.IsNullOrWhiteSpace(CurrentUser.Email) && !string.IsNullOrWhiteSpace(CurrentUser.Password);
        }
    }
}
