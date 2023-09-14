using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSK_WebAPI.DB.DBObjects;

public class Token
{
    [Key]
    public string TokenString { get; set; }
    [Key]
    public string TokenPermissionId { get; set; } // -> TokenPermission.TokenPermissionId
    [ForeignKey("TokenPermissionId")]
    public TokenPermission TokenPermission { get; set; }
}
