using System.ComponentModel.DataAnnotations;

namespace CVAnalyzer.Models.Requests
{
    public record VacancyRequest
    {
        public string? Link { get; set; }
        
        [Required]
        public Guid CvId { get; set; }
    };
}