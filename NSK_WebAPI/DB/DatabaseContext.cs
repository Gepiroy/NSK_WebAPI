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
        public static void Init()
        {
            var db = new DatabaseContext();
/*#if DEBUG
            db.Database.EnsureDeleted();
#endif*/

            var exists = db.Database.EnsureCreated();
            
            if(!exists)
            {
                // TODO: логика для первичного заполнения вспомогательных табличек
            }
        }

        public DbSet<User> Users { get; set; }
        /*public DbSet<Travel> Travels { get; set; }
        public DbSet<Transportation> Transportations { get; set; }
        public DbSet<Auto> Autos { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<CardType> CardTypes { get; set; }*/
        public DbSet<Token> Tokens { get; set; }
        public DbSet<TokenGroup> TokenGroups { get; set; }
        public DbSet<TokenGroupPermission> TokenGroupPermissions { get; set; }

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

            //modelBuilder.Entity<Token>()
            //    .HasOne(token => token.TokenGroup)
            //    .WithMany()
            //    .HasForeignKey(token => token.TokenGroupTitle)
            //    .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=usersdb;Username=postgres;Password=");
        }

        /*
         * Всё что здесь - должно быть синхронно. Сам метод лежит в отдельном потоке.
         */

        public static void Execute(Action<DatabaseContext> action)
        {
            var db = new DatabaseContext();
            action(db);
            db.Dispose();
        }

        public static T ExecuteAndReturn<T>(Func<DatabaseContext, T> action)
        {
            var db = new DatabaseContext();

            try
            {
                db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

                var result = action(db);
                return result;
            }
            finally
            {
                //db.Dispose();
            }
        }

        public static void ExecuteAsync(Action<DatabaseContext> action)
        {
            Execute(action); //TODO Асинхрон реализуем здесь позже. Он не нужен для запросов типа Get, поэтому разделил.
            //Кстати, а сами методы гетов/путов случайно не асинхронно вызываются?
        }
    }
}
