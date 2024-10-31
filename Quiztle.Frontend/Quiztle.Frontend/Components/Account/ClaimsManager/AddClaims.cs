using Microsoft.AspNetCore.Authentication;
using Quiztle.Frontend.Client.APIServices;
using System.Security.Claims;

namespace Quiztle.Frontend.Components.Account.ClaimsManager
{
    public class AddClaims : IClaimsTransformation
    {
        private readonly PaidService _paidService;

        public AddClaims(PaidService paidService) => _paidService = paidService;

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
            if (string.IsNullOrEmpty(emailClaim)) return principal;

            var paidRecords = await _paidService.GetPaidByEmailService(emailClaim);

            var claimType = "PriceId";
            if (paidRecords != null && paidRecords.Any() && !principal.HasClaim(c => c.Type == claimType))
            {
                foreach (var paidItem in paidRecords)
                {
                    principal.AddIdentity(new ClaimsIdentity(
[
                    new Claim(claimType, paidItem.PriceId!)
                ]));
                }
            }

            return principal;
        }

        public Task<ClaimsPrincipal> TransformAsync()
        {
            var principal = new ClaimsPrincipal();

            // Chama o método original para processar o ClaimsPrincipal vazio
            var resultPrincipal = TransformAsync(principal).Result;

            // Lista todos os claims
            Console.WriteLine("Claims:");
            foreach (var claim in resultPrincipal.Claims)
            {
                Console.WriteLine($"Type: {claim.Type}, Value: {claim.Value}");
            }

            // Lista todos os roles (assumindo que os roles estão definidos como claims do tipo ClaimTypes.Role)
            Console.WriteLine("Roles:");
            var roles = resultPrincipal.FindAll(ClaimTypes.Role);
            foreach (var role in roles)
            {
                Console.WriteLine(role.Value);
            }

            return Task.FromResult(resultPrincipal);
        }

    }
}
