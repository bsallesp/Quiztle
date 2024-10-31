//using Microsoft.AspNetCore.Authentication;
//using Quiztle.Frontend.Client.APIServices;
//using System.Security.Claims;

//namespace Quiztle.Frontend.Components.Account.ClaimsManager
//{
//    public class AddClaimsAutomatic : IClaimsTransformation
//    {
//        private readonly AddClaims _addClaims;

//        public AddClaimsAutomatic(AddClaims addClaims) => _addClaims = addClaims;

//        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
//        {
//            return await _addClaims.TransformAsync(principal);
//        }
//    }
//}
