using System.ComponentModel.DataAnnotations;

namespace NSK_WebAPI.DB.RequestObjects;

public class RequestRegUser
{//Для идеального определения типов данных (в частности, даты без времени) нужны такие вот модели под тело.
    public string FirstName { get; set; } //Имя
    public string LastName { get; set; } //Фамилия
    public string Patronymic { get; set; } //Отчество.
    [DataType(DataType.Date)]
    public DateTime BirthDay { get; set; } //Must be happy :3
    public string PhoneOrMail { get; set; } // nullable
    public string Password { get; set; } // nullable
}