using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("RefreshTokens")]
    public class DbRefreshToken
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        [Required]
        public string Value { get; set; }
        
        public DateTime ExpirationDate { get; set; }
        
        public bool IsRevoked { get; set; }
        
        [InverseProperty("RefreshTokens")]
        public DbUser User { get; set; }
    }
}