using System.ComponentModel.DataAnnotations;

namespace NSK_WebAPI.DB.RequestObjects;

public class RequestLoginByPassword
{//Для идеального определения типов данных (в частности, даты без времени) нужны такие вот модели под тело.
    public string? Phone { get; set; } // nullable
    public string? Email { get; set; } // nullable
    public string Password { get; set; } // nullable
}