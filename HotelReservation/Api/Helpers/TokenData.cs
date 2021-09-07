using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HotelReservation.Api.Helpers
{
    public  class TokenData
    {
        public static string GetIdFromClaims(IEnumerable<Claim> claims)
        {
            var idClaim = claims.FirstOrDefault(x =>
                    x.Type.Equals("id", StringComparison.InvariantCultureIgnoreCase))
                ?.Value ?? string.Empty;
            return idClaim;
        }
    }
}
