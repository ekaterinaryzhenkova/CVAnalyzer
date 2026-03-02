namespace CVAnalyzer.Models.Responses
{
    public record LoginResultResponse
    {
        /// <summary>
        /// User global unique identifier.
        /// </summary>
        public Guid UserId { get; init; }

        /// <summary>
        /// User access JWT.
        /// </summary>
        public string AccessToken { get; init; }
        
        //public string RefreshToken { get; init; }

        /// <summary>
        /// AccessToken life time in minutes.
        /// </summary>
        public double AccessTokenExpiresIn { get; init; }
        
        //public double RefreshTokenExpiresIn { get; init; }
    }
}