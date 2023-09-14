using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZubrServer.DB.DBObjects;
public class Card
{
    public int Id { get; set; } // -> User.UID
    public string CardNumber { get; set; }
}
