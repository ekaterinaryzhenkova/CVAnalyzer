using CVAnalyzer.DbLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace CVAnalyzer.DbLayer;

public class CVAnalyzerContext(
    DbContextOptions<CVAnalyzerContext> options)
    : DbContext(options)
{
    public DbSet<DbUser> Users { get; init; }
    
    public DbSet<DbCV> CVs { get; init; }
    
    public DbSet<DbLetter> Letters { get; init; }
    
    public DbSet<DbAnalysis> Analyses { get; init; }
    
    public DbSet<DbPrompt> Prompts { get; init; }
}