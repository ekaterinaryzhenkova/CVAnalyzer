using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public struct VacancyRequest
    {
        [Required]
        public string link { get; init; }
    }
}