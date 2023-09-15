using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NSK_WebAPI.DB.DBObjects;

[PrimaryKey("UserId")]
public class User
{
    //[Key]
    public int UserId { get; set; }
    // CardNumber не нужен, т.к. генерируется из id и в него же дешифруется. Поиск по id гораздо эффективнее чем по номеру карты, второй никогда не будет применён.
    // Хорошо, мой сладкий генератор
    public string FirstName { get; set; } //Имя
    public string LastName { get; set; } //Фамилия
    public string Patronymic { get; set; } //Отчество. Вписать сюда MiddleName - приведёт к человеческим ошибкам с большей вероятностью, чем Patronymic, т.к. мозг не участвует.
    
    [DataType(DataType.Date)]
    [Column(TypeName = "date")] 
    public DateTime BirthDay { get; set; } //Must be happy :3
    public string? PhoneNumber { get; set; } // nullable
    public string? Email { get; set; } // nullable
    public string? PassHash { get; set; } // nullable
    
    public bool IsFrozen { get; set; } = false; //Нет никаких состояний кроме "заморожена / не заморожена" // 👍👍
}
