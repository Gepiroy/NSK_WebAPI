using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NSK_WebAPI.DB.DBObjects;
using Microsoft.AspNetCore.Mvc;
using NSK_WebAPI.Controllers;

namespace NSK_WebAPI.DB
{
    public class DatabaseContext : DbContext
    {
        public static void Init()
        {
            LocalDBAPI.LoadConnectionString();
            
            var db = new DatabaseContext();
#if DEBUG
            db.Database.EnsureDeleted();
#endif

            var justCreated = db.Database.EnsureCreated();
            
            if(justCreated)
            {
                // Lогика для первичного заполнения вспомогательных табличек
                
                var groupAdmin = new TokenGroup { TokenGroupTitle = "Admin" };
                db.TokenGroups.Add(groupAdmin);

                var groupUser = new TokenGroup { TokenGroupTitle = "User" };
                db.TokenGroups.Add(groupUser);

                db.TokenGroupPermissions.Add(new TokenGroupPermission {TokenGroup = groupAdmin, Permission = Permissions.PERM_ADMIN});
                db.Tokens.Add(new Token {TokenString = "admin", TokenGroup = groupAdmin});

                db.SaveChanges();
                
                LocalDBAPI.RegisterUser(new User { FirstName = "Peter", LastName = "Parker", Patronymic = "Spiderovich", Email = "spiderman@mail.ru", BirthDay = DateTime.Today-TimeSpan.FromDays(19*365), PassHash = UsersController.MakeHash("123456")});
                LocalDBAPI.RegisterUser(new User { FirstName = "Harry", LastName = "Potter", Patronymic = "Hermionich", Email = "harry@hogvartz.ru", BirthDay = DateTime.Today-TimeSpan.FromDays(12*365), PassHash = UsersController.MakeHash("123456")});
                LocalDBAPI.RegisterUser(new User { FirstName = "Maxim", LastName = "Ded", Patronymic = "Pomerovich", PhoneNumber = "8-800-555-35-35", BirthDay = DateTime.Today-TimeSpan.FromDays(84*365), PassHash = UsersController.MakeHash("123456")});
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
            
            // Value generators
            modelBuilder.Entity<User>()
                .Property(user => user.UserId)
                .ValueGeneratedOnAdd();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(LocalDBAPI.ConnectionString);
        }

        public static void Execute(Action<DatabaseContext> action)
        {
            var db = new DatabaseContext();
            action(db);
            db.Dispose();
        }

        public static ActionResult Execute(Func<DatabaseContext, Token, ActionResult> action, string tokenString, params string[] permissions)
        {
            var db = new DatabaseContext();
            var token = Permissions.TokenFromString(db, tokenString);
            if (token is null) return new UnauthorizedResult();
            if(!Permissions.CheckPermissions(db, token, permissions))return new ForbidResult();
            return action(db, token);
        }

        public static T ExecuteAndReturn<T>(Func<DatabaseContext, T> action)
        {
            var db = new DatabaseContext();
            return action(db);
        }
        public static ActionResult<T> ExecuteAndReturn<T>(Func<DatabaseContext, Token, ActionResult<T>> action, string tokenString, params string[] permissions)
        {
            var db = new DatabaseContext();
            var token = Permissions.TokenFromString(db, tokenString);
            if (token is null)
            {
                Console.WriteLine("not found token "+tokenString);
                return new UnauthorizedResult();
            }

            if (!Permissions.CheckPermissions(db, token, permissions))
            {
                Console.WriteLine("permissions denied for "+tokenString);
                return new ForbidResult();
            }
            Console.WriteLine("success basecheck for "+tokenString);
            return action(db, token);
        }
    }
}
