using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSK_WebAPI.DB.DBObjects;

[PrimaryKey("TokenGroupPermissionId")]
public class TokenGroupPermission
{
    public int TokenGroupPermissionId { get; set; }

    [ForeignKey("TokenGroup")]
    public int TokenGroupId { get; set; } // -> TokenGroup.TokenGroupId;
    public TokenGroup TokenGroup { get; set; }

    public string Permission { get; set; }
}
