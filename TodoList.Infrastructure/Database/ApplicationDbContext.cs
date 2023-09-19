using Microsoft.EntityFrameworkCore;
using TodoList.Domain.TodoItem;

namespace TodoList.Infrastructure.Database
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoItem>()
                .Property(e => e.IsIncomplete)
                .HasDefaultValue(true);

            modelBuilder.Entity<TodoItem>()
                .Property(e => e.CreatedDate)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}