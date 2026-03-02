using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record RegisterRequest(
        string Name,
        string Login,
        string Password
    );
}