using Microsoft.EntityFrameworkCore;

namespace Database.Basic
{
    public class UserDbContext: DbContext
    {
        public DbSet<Department> Departments { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(@"Data Source=UserDB.db");
            }
        }
    }
}
