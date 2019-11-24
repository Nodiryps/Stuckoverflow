using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using prid1920_g10.Models;
using PRID_Framework;

namespace prid1920_g10.Models
{
    public class G10Context : DbContext
    {
        public DbSet<User> Users { get; set; }

        public G10Context(DbContextOptions<G10Context> options)
            : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().HasData(
                new User
                {
                    Id = 1, Pseudo = "admin", Password = "epfc", Email = "admin@epfc.eu",
                    FirstName = "Nimda", LastName = "Rotartsi", BirthDate = new DateTime(1990, 1, 27),
                    Role = Role.Admin
                },
                new User
                {
                    Id = 2, Pseudo = "yas", Password = "epfc", Email = "yas@epfc.eu",
                    FirstName = "Yasmina", LastName = "El Ghouate", BirthDate = new DateTime(1983, 1, 27)
                },
                new User
                {
                    Id = 3, Pseudo = "oth", Password = "epfc", Email = "oth@epfc.eu",
                    FirstName = "Othman", LastName = "Zamzam", BirthDate = new DateTime(2000, 1, 27)
                }
            );

            builder.Entity<User>()
                .Property(f => f.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn(); 
            builder.Entity<User>()
                .HasIndex(u => u.Pseudo)
                .IsUnique(); //unicity pseudo
            builder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique(); //unicity email

            // if (builder.Entity<User>().Property(u => u.FirstName != ""))
            // {
            //     builder.Entity<User>().Property(b => b.FirstName).IsRequired();
            //     builder.Entity<User>().Property(b => b.LastName).IsRequired();
            // }

            // if (builder.Entity<User>().Property(u => u.LastName != ""))
            // {
            //     builder.Entity<User>().Property(b => b.FirstName).IsRequired();
            //     builder.Entity<User>().Property(b => b.LastName).IsRequired();
            // }
        }
    }
}