using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NSK_WebAPI.DB.DBObjects;
namespace NSK_WebAPI.DB
{
    public class DatabaseContext : DbContext
    {
        private static DatabaseContext globalDbContext;

        public static void Init()
        {
            globalDbContext = new();
        }

        // Теперь можно юзать один контекст всем))

        public static DatabaseContext LockContext()
        {
            Monitor.Enter(globalDbContext);
            return globalDbContext;
        }

        public static void ReleaseContext()
        {
            if(Monitor.IsEntered(globalDbContext))
            {
                Monitor.Pulse(globalDbContext);
                Monitor.Exit(globalDbContext);
            }
        }

        public DbSet<User> Users { get; set; }
        /*public DbSet<Travel> Travels { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Auto> Autos { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardType> CardTypes { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenPermission> TokenPermissions { get; set; }*/

        public DatabaseContext()
        {
            #if DEBUG
                Database.EnsureDeleted();
            #endif

            var exists = Database.EnsureCreated();

            if(!exists)
            {
                // TODO: логика для первичного заполнения вспомогательных табличек
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder){
            base.OnModelCreating(modelBuilder);
            
            // Primary keys and value generators

            //modelBuilder.Entity<User>()
            //    .HasKey(user => user.UserId); //Primary keys adding. Probably there's more options like references and defaults, but IDK for now. // actually it's ”constraints” not “references” when speaking about DBs <3
            modelBuilder.Entity<User>()
                .Property(user => user.UserId)
                .ValueGeneratedOnAdd();

            //modelBuilder.Entity<Travel>()
            //    .HasKey(travel => travel.TravelId);
            /*modelBuilder.Entity<Travel>()
                .Property(p => p.TravelId)
                .ValueGeneratedOnAdd();

            //modelBuilder.Entity<Transportation>()
            //    .HasKey(transportation => transportation.TransportationId);
            modelBuilder.Entity<Transportation>()
                .Property(p => p.TransportationId)
                .ValueGeneratedOnAdd();

            //modelBuilder.Entity<Auto>()
            //    .HasKey(auto => auto.AutoId);
            modelBuilder.Entity<Auto>()
                .Property(p => p.AutoId)
                .ValueGeneratedOnAdd();*/

            //modelBuilder.Entity<Card>()
            //    .HasKey(card => new { card.UserId, card.CardNumber });

            //modelBuilder.Entity<Token>()
            //    .HasKey(token => new { token.TokenString, token.TokenPermissionId });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=");
        }
    }
}
