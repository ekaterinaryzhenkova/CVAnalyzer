using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("UsersCredentials")]
    public class DbUserCredentials
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [ForeignKey("User")]
        public Guid UserId { get; set; }
        
        [Required]
        public string Login { get; set; }
        
        [Required]
        public string PasswordHash { get; set; }
        
        [InverseProperty("UsersCredentials")]
        public DbUser User { get; set; }
    }
}