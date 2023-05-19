using Microsoft.EntityFrameworkCore;

namespace CoriaToDo.API.Data
{
    public class ToDoDbContext : DbContext
    {
        public ToDoDbContext(DbContextOptions<ToDoDbContext> options) : base(options)
        {
        }

        public DbSet<ToDoItem> ToDoItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<ToDoItem>()
                .Property(t => t.CreatedDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        }
    }
}
