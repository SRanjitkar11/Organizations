using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TestOrganization.Models;

namespace TestOrganization.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationUser> OrganizationUsers { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Designation> Designations { get; set; }
        public DbSet<LeaveRequest> LeaveRequests { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users");
            this.SeedUsers(builder);
            this.SeedRoles(builder);
            this.SeedDesignations(builder);
            builder.Entity<OrganizationUser>().HasKey("UserId", "OrganizationId");
        }
        private void SeedUsers(ModelBuilder builder)
        {
            ApplicationUser user = new ApplicationUser()
            {
                Id = Guid.NewGuid().ToString(),
                UserName = "Admin",
                NormalizedUserName = "ADMIN",
                Email = "superadmin@gmail.com",
                NormalizedEmail = "superadmin@gmail.com".ToUpper(),
                LockoutEnabled = false,
                IsAdmin = true,
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            var hashed = passwordHasher.HashPassword(user, "Admin1#");
            user.PasswordHash = hashed;

            builder.Entity<ApplicationUser>().HasData(user);
        }

        private void SeedRoles(ModelBuilder builder)
        {
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN", ConcurrencyStamp = Guid.NewGuid().ToString() });
            builder.Entity<IdentityRole>().HasData(new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER", ConcurrencyStamp = Guid.NewGuid().ToString() });

        }

        public void SeedDesignations(ModelBuilder builder)
        {
            builder.Entity<Designation>().HasData(new Designation { Id = Guid.NewGuid().ToString(), Name = "Project Manager", CallName = "PM" });
            builder.Entity<Designation>().HasData(new Designation { Id = Guid.NewGuid().ToString(), Name = "Team Lead", CallName = "TL" });
            builder.Entity<Designation>().HasData(new Designation { Id = Guid.NewGuid().ToString(), Name = "Developer", CallName = "Developer" });
        }
    }
}
