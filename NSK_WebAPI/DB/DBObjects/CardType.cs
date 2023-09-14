using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class CardType
{
    [Key]
    public int CardTypeId { get; set; }
    public string Type { get; set; }
}
