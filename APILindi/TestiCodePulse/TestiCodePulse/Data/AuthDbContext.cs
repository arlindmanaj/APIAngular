using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace TestiCodePulse.Data
{
    public class AuthDbContext : IdentityDbContext
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Generated with C# Interactive window
            var readerRoleId = "125d25ea-bf9c-4821-9905-394e981a76e5";
            var writerRoleId = "5ada7075-1055-4651-8ac8-5866fc4f8f7c";
            // Create Reader and Writer Role 


            var roles = new List<IdentityRole>
            {
                new IdentityRole()
                {
                    Id = readerRoleId,
                    Name = "Reader",
                    NormalizedName = "Reader".ToUpper(),
                    ConcurrencyStamp = readerRoleId
                },

                new IdentityRole()
                {
                    Id = writerRoleId,
                    Name = "Writer",
                    NormalizedName = "Writer".ToUpper(),
                    ConcurrencyStamp = writerRoleId
                }
            };

            // Seeding me store predefined data for your project so it sets up a functionality stream, It kinda tells your project "Hey this is where youre going" and 
            // seeding tests it
            //Seed the roles

            builder.Entity<IdentityRole>().HasData(roles);

            // Create an Admin User

            var adminUserId = "aaf69a2e-b507-45d4-b0ea-4ac96ddebd8e";
            var admin = new IdentityUser()
            {
                Id = adminUserId,
                UserName = "admin@projekti.com",
                Email = "admin@projekti.com",
                NormalizedEmail = "admin@projekti.com".ToUpper(),
                NormalizedUserName = "admin@projekti.com".ToUpper()

            };

            // Seed password 
            admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");
            builder.Entity<IdentityUser>().HasData(admin);

            // Give Roles to Admin 

            var adminRoles = new List<IdentityUserRole<string>>()
            {
                new()
                {
                    UserId = adminUserId,
                    RoleId = readerRoleId

                },
                new()
                {
                     UserId = adminUserId,
                    RoleId = writerRoleId  
                }
            };

            builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
            
        }
    }
}
