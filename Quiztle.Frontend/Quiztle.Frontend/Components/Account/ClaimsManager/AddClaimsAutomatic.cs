using Microsoft.AspNetCore.Authentication;
using Quiztle.Frontend.Client.APIServices;
using System.Security.Claims;

namespace Quiztle.Frontend.Components.Account.ClaimsManager
{
    public class AddClaimsAutomatic : IClaimsTransformation
    {
        private readonly PaidService _paidService;
        private readonly AddClaims _addClaims;

        public AddClaimsAutomatic(PaidService paidService)
        {
            _paidService = paidService;
            _addClaims = new AddClaims(_paidService);
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            principal = await _addClaims.TransformAsync(principal);

            return principal;
        }
    }
}
