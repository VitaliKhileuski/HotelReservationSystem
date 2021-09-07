namespace Business
{
    public class AuthOptions
    {
        public const string Authentication = "AuthenticationOptions";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int Lifetime { get; set; }
        public string SecretKey { get; set; }
        public bool ValidateLifetime { get; set; }
        public bool ValidateIssuer {get; set;}
        public bool ValidateAudience { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
    }
}