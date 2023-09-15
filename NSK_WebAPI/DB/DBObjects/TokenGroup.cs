using System.ComponentModel.DataAnnotations;

namespace NSK_WebAPI.DB.DBObjects;

public class TokenGroup
{
    [Key]
    public int Id { get; set; } //Тупо уникальный номер, чтобы записи отличаться умели.
    [Key]
    public int TokenGroupId { get; set; }
    public string Permission { get; set; }
}
