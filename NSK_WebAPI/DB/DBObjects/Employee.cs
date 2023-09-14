using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZubrServer.DB.DBObjects;
public class Employee
{
    public int employeeId { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public int roleId { get; set; } // -> Role.roleId
}
