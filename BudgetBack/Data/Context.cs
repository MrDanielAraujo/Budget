using Microsoft.EntityFrameworkCore;

namespace BudgetBack.Data;

public class Context : DbContext
{
      public Context(DbContextOptions<Context> options) : base(options) { }
      
      protected override void OnModelCreating(ModelBuilder builder)
      {
            base.OnModelCreating(builder);
      }

      protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
      {
            optionsBuilder.LogTo(Console.WriteLine);
      }
      
}