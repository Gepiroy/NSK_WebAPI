using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NSK_WebAPI.DB.DBObjects;

public class Token
{
    [Key]
    public string TokenString { get; set; }
    public string TokenGroupId { get; set; } // -> TokenGroups.TokenGroupId
}
