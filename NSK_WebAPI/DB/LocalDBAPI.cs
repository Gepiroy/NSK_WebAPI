using System.Runtime.Intrinsics.Arm;
using NSK_WebAPI.Controllers;
using NSK_WebAPI.DB.DBObjects;

namespace NSK_WebAPI.DB;

public static class LocalDBAPI //Лучше переименовать
{
    public static void RegisterUser(User user)
    {
        var db = new DatabaseContext();
        db.Users.Add(user);
        db.Tokens.Add(new Token { User = user, TokenString = GenerateToken(), TokenGroup = db.TokenGroups.First(g => g.TokenGroupId == 2) });
        db.SaveChanges();
        db.DisposeAsync();
    }

    private static Random _random = new Random();
    static string GenerateToken()
    {
        const string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#%^&*+-_=~";
        return new string(Enumerable.Repeat(chars, 12)
            .Select(s => s[_random.Next(s.Length)]).ToArray());
    }
}