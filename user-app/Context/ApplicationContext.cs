using user_app.Models;
using Microsoft.EntityFrameworkCore;

namespace user_app.Context;

public class ApplicationContext: DbContext
{
    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users => Set<User>();
    public DbSet<UserGroup> UserGroups => Set<UserGroup>();
    public DbSet<UserState> UserStates => Set<UserState>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.HasOne(_ => _.UserGroup).WithMany();
            user.HasOne(_ => _.UserState).WithMany();
            user.Property(_ => _.CreatedDate).HasDefaultValueSql("now()");
            user.HasIndex(_ => _.Login).IsUnique();
        });
        modelBuilder.Entity<UserGroup>(g =>
        {
            g.HasData(new UserGroup { UserGroupId = Guid.NewGuid(), Code = GroupCode.Admin, Description = "Admin role" }, new UserGroup { UserGroupId = Guid.NewGuid(), Code = GroupCode.User, Description = "User role" });
            g.HasIndex(_ => _.Code).IsUnique();
        });
        modelBuilder.Entity<UserState>(s =>
        {
            s.HasData(new UserState { UserStateId = Guid.NewGuid(), Code = StateCode.Active, Description = "Active user status"}, new UserState { UserStateId = Guid.NewGuid(), Code = StateCode.Blocked, Description = "Blocked user status" });
            s.HasIndex(_ => _.Code).IsUnique();
        });
    }
}