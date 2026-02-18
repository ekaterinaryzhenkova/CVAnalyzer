using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("CVs")]
    public class DbCV
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        public string ParsedText { get; set; }
        
        [InverseProperty("CV")]
        public DbUser User { get; set; }
    }
}