using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ConwayWebApi.Database
{
    public class ConwayContext : DbContext
    {
        public ConwayContext(DbContextOptions<ConwayContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; } = null!;
    }
}