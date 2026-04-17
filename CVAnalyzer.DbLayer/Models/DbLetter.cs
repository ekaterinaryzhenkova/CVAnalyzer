using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("Letters")]
    public class DbLetter
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("Analysis")]
        public Guid AnalysisId { get; set; }
        
        public string Text { get; set; }
        
        [InverseProperty("Letter")]
        public DbAnalysis Analysis { get; set; }
    }
}