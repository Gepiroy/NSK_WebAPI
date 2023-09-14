using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZubrServer.DB.DBObjects;
public class Partner
{
    public int partnerId { get; set; }
    public string token { get; set; }
    public string name { get; set; }
    public string partnerRoleId { get; set; }
}
