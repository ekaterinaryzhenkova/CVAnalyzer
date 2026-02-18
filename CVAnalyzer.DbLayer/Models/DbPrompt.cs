using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("Prompts")]
    public class DbPrompt
    {
        [Key]
        public Guid Id { get; set; }
        
        public string Name { get; set; }
        
        public string Content { get; set; }
        
        public bool IsActive { get; set; }
        
        public DateOnly CreatedAt { get; set; }
        
        public DateOnly? UpdatedAt { get; set; }
    }
}