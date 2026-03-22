using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record RefreshRequest
    {
        public string RefreshToken { get; set; }
    };
}