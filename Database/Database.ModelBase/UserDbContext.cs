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
                // 设置索引
                entity.HasIndex(e => e.Username);

                // 用户名是必须的
                entity.Property(e => e.Username)
                    .IsRequired();

                // 部门 Id 是必须的
                entity.Property(e => e.DepartmentId)
                    .IsRequired();
            });

            #endregion


            #region 配置多对多

            modelBuilder.Entity<Role>()
                .HasMany(r => r.P)
                .WithMany(p => p.R)
                .UsingEntity(x =>
                {
                    x.ToTable("my_role_permission");
                    x.Property<int>("PId").HasColumnType("int").HasColumnName("p_id");
                    x.Property<int>("RId").HasColumnType("int").HasColumnName("r_id");
                });

            #endregion


            #region 分组配置

            new UserEntityTypeConfiguration().Configure(modelBuilder.Entity<User>());
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserEntityTypeConfiguration).Assembly);

            #endregion
        }
    }
}
