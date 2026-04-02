using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("CVs")]
    public class DbCV
    {
        [Key]
        public Guid Id { get; set; }
        
        [ForeignKey("User")]
        public Guid? UserId { get; set; }
        
        public string ParsedText { get; set; }
        
        [InverseProperty("CVs")]
        public DbUser User { get; set; }
        
        [InverseProperty("CV")]
        public DbAnalysis Analysis { get; set; }
    }
}