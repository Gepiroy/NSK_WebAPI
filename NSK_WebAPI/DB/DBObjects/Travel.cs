using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZubrServer.DB.DBObjects;
public class Travel
{
    public int idTravel { get; set; }
    public int UID { get; set; } // -> User.UID
    public DateTime date { get; set; }
    public int transId { get; set; } // -> Transportation.transId
}
