using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class Card
{
    [Key]
    public int UserId { get; set; } // -> User.UserId
    [ForeignKey("UserId")]
    public User User { get; set; }

    [Key]
    public string CardNumber { get; set; }

    public int CardTypeId { get; set; } // -> CardType.CardTypeId
    [ForeignKey("CardTypeId")]
    public CardType CardType { get; set; }
}
