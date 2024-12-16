using Auth.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Auth.Dal
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseNpgsql("Server=mathapp-tests.postgres.database.azure.com;Database=postgres;Port=5432;User Id=mathapp;Password=projektZespolowy123;Ssl Mode=Require;");
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id)
                    .IsRequired()
                    .HasMaxLength(32);

                entity.Property(u => u.Mail)
                    .IsRequired()
                    .HasMaxLength(254);

                entity.Property(u => u.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(88);
            });
        }
    }
}
