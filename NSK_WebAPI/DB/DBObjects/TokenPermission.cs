using System.ComponentModel.DataAnnotations;

namespace NSK_WebAPI.DB.DBObjects;

public class TokenPermission
{
    [Key]
    public int TokenPermissionId { get; set; }
    public string TokenPermissionName { get; set; }
}
