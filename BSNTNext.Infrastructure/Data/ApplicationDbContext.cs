using BSNTNext.Domain.Entity;
using BSNTNext.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BSNTNext.Infrastructure.Data
{
    public class ApplicationDbContext 
        : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<SeedHistory> SeedHistory { get; set; }
        public DbSet<AppModule> AppModules { get; set; }
        public DbSet<ModulePermission> ModulePermissions { get; set; }
        public DbSet<UserModuleAccess> UserModuleAccesses { get; set; }
        public DbSet<UserModulePermission> UserModulePermissions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<AppModule>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Name).IsRequired();
                entity.Property(x => x.DisplayName).IsRequired();
            });

            builder.Entity<ModulePermission>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Module)
                      .WithMany() 
                      .HasForeignKey(x => x.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<UserModuleAccess>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.Module)
                      .WithMany(m => m.UserAccesses) 
                      .HasForeignKey(x => x.ModuleId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<UserModulePermission>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.HasOne(x => x.ModulePermission)
                      .WithMany(m => m.UserPermissions) 
                      .HasForeignKey(x => x.ModulePermissionId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne<ApplicationUser>()
                      .WithMany()
                      .HasForeignKey(x => x.UserId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}