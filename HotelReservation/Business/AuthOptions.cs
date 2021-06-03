using System;
using System.Collections.Generic;
using System.Text;

namespace Business
{
   public class AuthOptions
    {
            public const string Authentication = "AuthenticationOptions";
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public int Lifetime { get; set; }
            public string SecretKey { get; set; }

    }
}