using System;
using System.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace RLTestTask.Models
{
    public class ApplicationContext : DbContext 
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        private IPasswordHasher<User> passwordHasher;
        public ApplicationContext(DbContextOptions<ApplicationContext> options, IPasswordHasher<User> passwordHasher)
            : base(options) 
        { 
            this.passwordHasher = passwordHasher;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder) 
        {
            modelBuilder
                .Entity<Role>()
                .HasData(new Role {Id = 1, Name="admin"},
                         new Role {Id = 2, Name="editor"},
                         new Role {Id = 3, Name="customer"},
                         new Role {Id = 4, Name="contractor"});
            
            User[] users = new User[] {
                new User 
                {
                    Id = 1,
                    Login = "admin",
                    Name = "Admin",
                    Email = "admin@admin.admin",
                    Password = "admin"
                },
                new User 
                {
                    Id = 2,
                    Login = "editor",
                    Name = "Editor",
                    Email = "editor@editor.editor",
                    Password = "editor"
                },
                new User 
                {
                    Id = 3,
                    Login = "customerContractor",
                    Name = "CustomerContractor",
                    Email = "custome@contractor.customer",
                    Password = "customercontractor"
                }
            };
            users = users.Select(u => 
                new User{
                    Id = u.Id,
                    Login = u.Login,
                    Name = u.Name,
                    Email = u.Email,
                    Password = passwordHasher.HashPassword(u,u.Password)
                })
                .ToArray();

            modelBuilder
                .Entity<User>()
                .HasData(users);
            
            modelBuilder
                .Entity<User>()
                .HasMany(u => u.Roles)
                .WithMany(r => r.Users)
                .UsingEntity(ur => ur.HasData(
                    new { UsersId = 1, RolesId = 1 },
                    new { UsersId = 2, RolesId = 2 },
                    new { UsersId = 3, RolesId = 3 },
                    new { UsersId = 3, RolesId = 4 }
                ));
        }
    }
}