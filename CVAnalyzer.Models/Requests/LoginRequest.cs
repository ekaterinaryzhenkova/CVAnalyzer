using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record LoginRequest(
        [property: Required] string LoginData,
        [property: Required] string Password);
}