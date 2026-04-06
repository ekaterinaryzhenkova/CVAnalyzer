using CVAnalyzer.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("Analyses")]
    public class DbAnalysis
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("CV")]
        public Guid CvId { get; set; }
        
        public string? VacancyLink { get; set; }
        
        public string? Structure { get; set; }
        
        public string? Technologies { get; set; }
        
        public string? Relevance { get; set; }

        public string? VacancyComparison { get; set; }

        public string? Another { get; set; }

        public AnalysisStatus Status { get; set; }
        
        public DateTime CreatedAt { get; set; }
        
        [InverseProperty("Analysis")]
        public DbCV CV { get; set; }
    }
}