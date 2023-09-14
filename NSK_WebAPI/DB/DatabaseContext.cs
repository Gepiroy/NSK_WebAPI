using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ZubrServer.DB.DBObjects;
namespace ZubrServer.DB
{
    internal class DatabaseContext : DbContext
    {
        public DbSet<User> Users { get; set; } = null!;
        /*public DbSet<Travel> Travels { get; set; } = null!;
        public DbSet<Transportation> Transportations { get; set; } = null!;
        public DbSet<Auto> Autos { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;
        public DbSet<Role> Roles { get; set; } = null!;
        public DbSet<State> States { get; set; } = null!;
        public DbSet<Card> Cards { get; set; } = null!;
        public DbSet<Partner> Partners { get; set; } = null!;
        public DbSet<PartnerRole> PartnersRoles { get; set; } = null!;*/ //Causes errors when primary key is not choosed.

        public DatabaseContext()
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder){
            modelBuilder.Entity<User>()
                .Property(p => p.Id)
                .ValueGeneratedOnAdd(); //Primary keys adding. Probably there's more options like references and defaults, but IDK for now.
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=пароль_от_postgres");
        }
    }
}
