using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;
public class Auto
{
    [Key]
    public int AutoId { get; set; }
    public string AutoNumber { get; set; }
}
