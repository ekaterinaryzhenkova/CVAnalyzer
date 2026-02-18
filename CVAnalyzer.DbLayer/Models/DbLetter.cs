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
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        public string Text { get; set; }
        
        [InverseProperty("Letters")]
        public DbUser User { get; set; }
    }
}