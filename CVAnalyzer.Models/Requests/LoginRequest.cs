using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record LoginRequest
    {
        public string Login { get; init; }
        
        public string Password { get; init; }
    };
}