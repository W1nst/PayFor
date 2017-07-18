using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PayFor.Models;

namespace PayFor.Context
{
    public class PayForContext: IdentityDbContext<User>
    {
        private readonly IConfigurationRoot _config;

        public PayForContext(IConfigurationRoot config, DbContextOptions options):base(options)
        {
            _config = config;
        }
        public DbSet<Payment> Payments { get; set; } 
        public DbSet<Group> Groups { get; set; }
        public DbSet<Category> Categories { get; set; }
        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserGroup>()
            .HasKey(t => new { t.UserId, t.GroupId });

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.User)
                .WithMany(p => p.UserGroups)
                .HasForeignKey(pt => pt.UserId);

            modelBuilder.Entity<UserGroup>()
                .HasOne(pt => pt.Group)
                .WithMany(t => t.UserGroups)
                .HasForeignKey(pt => pt.GroupId);
            
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(_config["ConnectionStrings:PayForConnection"]);
        }
    }   
}
