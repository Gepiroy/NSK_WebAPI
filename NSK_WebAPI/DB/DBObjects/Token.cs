using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSK_WebAPI.DB.DBObjects;

[PrimaryKey("TokenString")]
public class Token
{
    public string TokenString { get; set; }

    [ForeignKey("User")]
    public int? UserId { get; set; } // -> User.UserId // nullable // Foreign key
    public User? User { get; set; } // Navigation

    [ForeignKey("TokenGroup")]
    public int TokenGroupId { get; set; } // -> TokenGroup.TokenGroupTitle (не ключ, будем уповать на Аллаха ) -- целостность данных? это кто?
    public TokenGroup TokenGroup { get; set; } // Navigation
}
