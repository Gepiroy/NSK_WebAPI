using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class User
{
    [Key]
    public int UserId { get; set; }
    public string CardNumber { get; set; }
    public string FirstName { get; set; }
    public string MiddleName { get; set; }
    public string LastName { get; set; }
    public string PhoneNumber { get; set; } // nullable
    public string Email { get; set; } // nullable
    public string PassHash { get; set; } // nullable

    public string TokenString { get; set; } // -> Token.TokenString
    [ForeignKey("TokenString")]
    public Token Token { get; set; }

    public string GroupId { get; set; } // -> Group.GroupId
    [ForeignKey("GroupId")]
    public Group Group { get; set; }

    public string StateId { get; set; } // -> State.StateId
    [ForeignKey("StateId")]
    public State State { get; set; }
}
