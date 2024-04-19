using BrunoTheBot.CoreBusiness;
using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace BrunoTheBot.Blazor.Client.Authentication.Core
{
    public class DefaultAuthenticationStateProvider : AuthenticationStateProvider
    {
        public User? CurrentUser { get; private set; } = new User { Email = "test@example.com", Password = "hashedPassword" };

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var principal = new ClaimsPrincipal();

            if (CurrentUser != null && CurrentUser.Email != null && CurrentUser.Password != null)
            {
                // Criar as claims do usuário
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, CurrentUser.Email)
                    // Adicione outras claims, se necessário
                };

                // Criar uma identidade baseada nas claims
                var identity = new ClaimsIdentity(claims, "authenticationType");

                // Criar um ClaimsPrincipal baseado na identidade
                principal = new ClaimsPrincipal(identity);
            }

            return Task.FromResult(new AuthenticationState(principal));
        }

        public void LogoutAsync()
        {
            // Limpar os dados do usuário atual
            CurrentUser = null;

            // Notificar sobre a mudança de autenticação
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(new ClaimsPrincipal())));
        }
    }
}
