using System.ComponentModel.DataAnnotations;

namespace NSK_WebAPI.DB.RequestObjects;

public class RequestUpdateUser
{
    public string? FirstName { get; set; } //Имя
    public string? LastName { get; set; } //Фамилия
    public string? Patronymic { get; set; } //Отчество.
    [DataType(DataType.Date)]
    public DateTime? BirthDay { get; set; } //Must be happy :3
    public string? PhoneNumber { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}