using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CVAnalyzer.DbLayer.Models
{
    [Table("Users")]
    public class DbUser
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        [InverseProperty("User")]
        public DbCV CV { get; set; }
        
        [InverseProperty("User")]
        public DbUserCredentials UsersCredentials { get; set; }
        
        [InverseProperty("User")]
        public List<DbRefreshToken> RefreshToken { get; set; } = [];

        [InverseProperty("User")]
        public List<DbLetter> Letters { get; set; } = [];
        
        [InverseProperty("User")]
        public List<DbAnalysis> Analyses { get; set; } = [];
    }
}