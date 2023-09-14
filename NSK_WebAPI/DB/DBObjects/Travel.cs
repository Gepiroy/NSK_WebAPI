using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class Travel
{
    [Key]
    public int TravelId { get; set; }

    public int UserId { get; set; } // -> User.UserId
    [ForeignKey("UserId")]
    public User User { get; set; }

    public DateTime Date { get; set; }

    public int TransportationId { get; set; } // -> Transportation.TransportationId
    [ForeignKey("TransportationId")]
    public Transportation Transportation { get; set; }
}
