using NSK_WebAPI.DB.DBObjects;

namespace NSK_WebAPI.DB;

public static class Permissions
{
    public const string PERM_ADMIN = "Admin";
    public static bool CanGetUser(DatabaseContext db, int userId, Token token)
    {
        if (token.UserId == userId) return true;
        
        return CheckPermissions(db, token, "GetUsers");
    }

    public static Token? TokenFromString(DatabaseContext db, string tokenString)
    {
        if(string.IsNullOrWhiteSpace(tokenString)) return null;
        return db.Tokens.FirstOrDefault(t => t.TokenString == tokenString);
    }
    
    /**
     * Token должен быть существующим и не null.
     * Проверка на неавторизованность выполняется в процессе конвертации токена.
     */
    public static bool CheckPermissions(DatabaseContext db, Token token, params string[] permissions)
    {
        var tokenPermissions = GetTokenPermissions(db, token);
        if (tokenPermissions.Any(st => st.Equals(PERM_ADMIN))) return true;
        foreach(var perm in permissions)
        {
            if (!tokenPermissions.Contains(perm)) return false;
        }
        return true;
    }

    static List<string> GetTokenPermissions(DatabaseContext db, Token token)
    {
        return db.TokenGroupPermissions
            .Where(p => p.TokenGroupId == token.TokenGroupId)
            .Select(p => p.Permission).ToList();
    }
}