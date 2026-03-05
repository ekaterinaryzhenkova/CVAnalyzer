namespace CVAnalyzer.Models.Token
{
    public class JwtOptions
    {
        public string Key { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int AccessTokenLifetimeInMinutes { get; set; }
        
        public int RefreshTokenLifetimeInMinutes { get; set; }
    }
}