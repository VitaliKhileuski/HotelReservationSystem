using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace HotelReservation.Api.Helpers
{
    public  class TokenData
    {
        public static int GetIdFromClaims(IEnumerable<Claim> claims)
        {
            var idClaim = int.Parse(claims.FirstOrDefault(x =>
                    x.Type.ToString().Equals("id", StringComparison.InvariantCultureIgnoreCase))
                ?.Value ?? string.Empty);
            return idClaim;
        }
    }
}
