using Microsoft.EntityFrameworkCore;

namespace NSK_WebAPI.DB.DBObjects;

[PrimaryKey("TokenGroupId")]
public class TokenGroup
{
    public int TokenGroupId { get; set; }
    public string TokenGroupTitle { get; set; }
}
