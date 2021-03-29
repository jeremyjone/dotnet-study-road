using Microsoft.EntityFrameworkCore;

namespace Database.ModelBase
{
    public class UserDbContext: DbContext
    {
        public DbSet<User> Users { get; set; }

        public UserDbContext(DbContextOptions<UserDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region 配置单属性

            modelBuilder.Entity<User>()
                .Property(user => user.Username)
                .IsRequired();

            #endregion


            #region 配置多个属性

            modelBuilder.Entity<User>(entity =>
            {
                // 用户名是必须的
                entity.Property(e => e.Username)
                    .IsRequired();

                // 部门 Id 是必须的
                entity.Property(e => e.DepartId)
                    .IsRequired();
            });

            #endregion


            #region 分组配置

            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

            #endregion
        }
    }
}
