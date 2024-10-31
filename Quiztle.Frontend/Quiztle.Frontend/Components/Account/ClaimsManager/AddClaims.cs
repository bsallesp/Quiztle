//using Quiztle.Frontend.Client.APIServices;
//using System.Security.Claims;

//namespace Quiztle.Frontend.Components.Account.ClaimsManager
//{
//    public class AddClaims
//    {
//        private readonly PaidService _paidService;

//        public AddClaims(PaidService paidService) => _paidService = paidService;

//        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
//        {
//            var emailClaim = principal.FindFirst(ClaimTypes.Email)?.Value;
//            if (string.IsNullOrEmpty(emailClaim)) return principal;

//            var paidRecords = await _paidService.GetPaidByEmailService(emailClaim);

//            var claimType = "PriceId";
//            if (paidRecords != null && paidRecords.Any() && !principal.HasClaim(c => c.Type == claimType))
//            {
//                foreach (var paidItem in paidRecords)
//                {
//                    principal.AddIdentity(new ClaimsIdentity(
//                    new[]
//                    {
//                        new Claim(claimType, paidItem.PriceId!)
//                    }));
//                }
//            }

//            return principal;
//        }

//        public async Task<ClaimsPrincipal> TransformAsync()
//        {
//            var emptyPrincipal = new ClaimsPrincipal();
//            return await TransformAsync(emptyPrincipal);
//        }
//    }
//}
