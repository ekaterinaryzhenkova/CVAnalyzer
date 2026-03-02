using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record VacancyRequest
    {
        [Required]
        public string Link { get; set; }
    };
}