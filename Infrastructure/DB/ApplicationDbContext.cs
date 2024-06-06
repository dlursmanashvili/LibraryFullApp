using Domain.AuthorEntity;
using Domain.BookAuthorEntity;
using Domain.BookEntity;
using Domain.RoleEntity;
using Domain.UserEntity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.DB;

public class ApplicationDbContext : IdentityDbContext<User, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Define keys for IdentityUserLogin
        modelBuilder.Entity<IdentityUserLogin<string>>().HasKey(login => new { login.LoginProvider, login.ProviderKey });

        // Define keys for IdentityUserRole
        modelBuilder.Entity<IdentityUserRole<string>>().HasKey(role => new { role.UserId, role.RoleId });

        // Define keys for IdentityUserToken
        modelBuilder.Entity<IdentityUserToken<string>>().HasKey(token => new { token.UserId, token.LoginProvider, token.Name });

        // Seed data for IdentityUserRole
        modelBuilder.Entity<IdentityUserRole<string>>().HasData(
            new IdentityUserRole<string>
            {
                UserId = "0f8fad5b-d9cb-429f-a165-70867528350e", // ID пользователя
                RoleId = "2c5e174e-3b0e-446f-86af-483d56fd7210" // ID роли
            });

        // Seed data for ApplicationRole
        modelBuilder.Entity<ApplicationRole>().HasData(
            new ApplicationRole
            {
                Id = "2c5e174e-3b0e-446f-86af-483d56fd7210",
                Name = "RootAdmin",
                NormalizedName = "ROOTADMIN",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
                ConcurrencyStamp = "11d16551-a780-477d-8b58-6091a7269cb3",
            },
            new ApplicationRole
            {
                Id = "847aeaad-ec0e-4b85-b20e-1828fae116c9",
                Name = "user",
                NormalizedName = "USER",
                CreatedAt = DateTime.MinValue,
                UpdatedAt = DateTime.MinValue,
                ConcurrencyStamp = "5fb7f941-9d65-4354-96ef-e201532c2f1f",
                DeletedAt = DateTime.MinValue,
            });

        // Seed data for User
        modelBuilder.Entity<User>().HasData(
            new User
            {
                Id = "0f8fad5b-d9cb-429f-a165-70867528350e",
                SecurityStamp = "d14d334e-0893-403d-81d3-486d2700aafa",
                UserName = "SuperAdmin@gmail.com",
                NormalizedUserName = "SUPERADMIN@GMAIL.COM",
                PasswordHash = "AQAAAAEAACcQAAAAEL9PxquaUB30HvHfKoBvT5a6X/YrmS7efzjNH6b+AoFZTLYBGDxjsO8xKOUI5iz7fQ==", // Password@123
                Email = "SuperAdmin@gmail.com",
                NormalizedEmail = "SUPERADMIN@GMAIL.COM",
                ConcurrencyStamp = "3bc03145-cc9e-4d45-9d5f-e38aa27a7a01",
                PNumber = "admin",
                FirstName = "admin",
                LastName = "admin",
                IsActive = true,
            });
    }
    public virtual DbSet<User> Users { get; set; }
    public virtual DbSet<Author> Authors{ get; set; }
    public virtual DbSet<Book> Books{ get; set; }
    public virtual DbSet<BookAuthor> BookAuthors{ get; set; }
}
