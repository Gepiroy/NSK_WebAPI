using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZubrServer.DB.DBObjects;
public class Transportation
{
    public int transId { get; set; }
    public int autoId { get; set; } // -> Auto.autoId
    public int employeeId { get; set; } // -> Employee.employeeId
    public DateTime date { get; set; }
}
