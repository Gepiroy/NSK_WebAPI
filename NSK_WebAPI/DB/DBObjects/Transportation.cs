using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class Transportation
{
    [Key]
    public int TransportationId { get; set; }

    public int AutoId { get; set; } // -> Auto.AutoId
    [ForeignKey("AutoId")]
    public Auto Auto { get; set; }

    public int UserId { get; set; } // -> User.UserId
    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime Date { get; set; }
}
