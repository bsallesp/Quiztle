using Quiztle.CoreBusiness;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Quiztle.Blazor.Client.Authentication.Core
{
    public class DefaultAuthenticationStateProvider : AuthenticationStateProvider
    {
        public User? CurrentUser { get; set; } = new User { Email = "test@example.com", Password = "hashedPassword" };

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();

            if (CurrentUser != null && !string.IsNullOrWhiteSpace(CurrentUser.Email) && !string.IsNullOrWhiteSpace(CurrentUser.Password))
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, CurrentUser.Email),
                    new Claim(ClaimTypes.Role, "User")
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
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, "User")
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
