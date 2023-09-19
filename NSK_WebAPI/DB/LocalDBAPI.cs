using System.Runtime.Intrinsics.Arm;
using System.Text;
using NSK_WebAPI.Controllers;
using NSK_WebAPI.DB.DBObjects;

namespace NSK_WebAPI.DB;

public static class LocalDBAPI //Лучше переименовать
{
    public static string ConnectionString="Host=localhost; Port=5432; Database=usersdb; Username=postgres; Password=;";
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

    public static void LoadConnectionString()
    {
        string filePath = "connection_settings.txt";
        if (File.Exists(filePath))
        {
            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    ConnectionString = reader.ReadLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке настроек подключения: {ex.Message}");
            }
        }
        else //Если настроек нет, нужно хоть бланк создать.
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    //Лучше сохранять весь connectionString чем отдельные параметры, т.к. в разных БД могут быть разные особые тэги подключения.
                    writer.WriteLine(ConnectionString);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при сохранении настроек подключения: {ex.Message}");
            }
                
        }
    }
}