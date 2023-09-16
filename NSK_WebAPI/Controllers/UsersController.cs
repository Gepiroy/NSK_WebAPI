using Microsoft.AspNetCore.Mvc;
using NSK_WebAPI.DB;
using NSK_WebAPI.DB.DBObjects;
using NSK_WebAPI.DB.RequestObjects;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace NSK_WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly ILogger<UsersController> _logger;

        public UsersController(ILogger<UsersController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Get the list of users.
        /// </summary>
        /// <param name="token"></param>
        /// <returns>The list of users.</returns>
        [HttpGet(Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> GetUsers([FromHeader(Name="Token")] string token) // Охренеть ты коллекционер
        {
            return DatabaseContext.ExecuteAndReturnWithPermissions(db => db.Users as IEnumerable<User>, token, "ReadOthers");
        }
        
        [HttpGet, Route("GetUser/{userId}")]
        public ActionResult<User> GetUser([FromRoute(Name="userId")]int userId, [FromHeader(Name="Token")] string token)
        {
            User user;
            var foundUser = DatabaseContext.ExecuteAndReturnWithPermissions(db => db.Users.Where(u => u.UserId == userId), token, "ReadOthers");
            
            if(foundUser.Result is not null and not OkResult)
            {
                var dbToken = DatabaseContext.ExecuteAndReturnWithPermissions(db => db.Tokens.First(t => t.TokenString == token && t.UserId != null), token, "ReadSelf");
                if(dbToken.Result is not null and not OkResult || dbToken.Value is null)
                {
                    return new ActionResult<User>(new ForbidResult());
                }
                else
                {
                    return new ActionResult<User>(DatabaseContext.ExecuteAndReturn(db => db.Users.First(u => u.UserId == dbToken.Value.UserId)));
                }
            }
            else if(!foundUser.Value.Any())
            {
                return new ActionResult<User>(new NotFoundResult());
            }
            else
            {
                return new ActionResult<User>(foundUser.Value.First());
            }
        }
        
        [HttpPut, Route("RegisterUser")]
        public ActionResult RegisterUser([FromBody]RequestRegUser data)
        {
            string result = "";
            
            DatabaseContext.Execute(db =>
            {
                User user = new User{FirstName = data.FirstName, LastName = data.LastName, Patronymic = data.Patronymic, BirthDay = data.BirthDay, PassHash = MakeHash(data.Password)};
                if (data.PhoneOrMail.Contains("@")) //TODO нормальный проверяльщик
                {
                    if (db.Users.Any(u => data.PhoneOrMail.Equals(u.Email))) //Гениальнейший способ избежать nullPointerException: поменять equals местами)
                    {
                        result = "Email already exists";
                        return;
                    }
                    user.Email = data.PhoneOrMail;
                    //TODO отсылание кода подтверждения на почту
                }
                else
                {
                    if (db.Users.Any(u => data.PhoneOrMail.Equals(u.PhoneNumber)))
                    {
                        result = "Phone already exists";
                        return;
                    }
                    user.PhoneNumber = data.PhoneOrMail; //TODO нормальный перешифровыватель в нужные форматы
                    //TODO отсылание СМС-подтверждения
                }
                db.Users.Add(user);
                db.SaveChanges();
            });
            
            if (result.Length > 0) return Conflict(result);
            return Ok();
        }
        [HttpPost, Route("UpdateUser/{userId}")]
        public ActionResult UpdateUser(int userId, [FromBody]RequestUpdateUser data)
        {
            string result = "";
            
            DatabaseContext.Execute(db =>
            {
                User user = db.Users.First(u => u.UserId == userId);

                if(data.FirstName is not null) user.FirstName = data.FirstName;
                if(data.LastName is not null) user.LastName = data.LastName;
                if(data.Patronymic is not null) user.Patronymic = data.Patronymic;
                if(data.BirthDay is not null) user.BirthDay = data.BirthDay.Value;
                if(data.Email is not null) user.Email = data.Email;
                if(data.PhoneNumber is not null) user.PhoneNumber = data.PhoneNumber;

                //TODO probably there is some way to do this automatically. Reflection?

                // Можно. Следи за руками
                //var fields = data.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
                //foreach(var field in fields)
                //{
                //    if(field.GetValue(data) != null)
                //    {
                //        var desiredField = user.GetType().GetField(field.Name, BindingFlags.Instance | BindingFlags.NonPublic);
                //        if(desiredField != null)
                //        {
                //            desiredField.SetValue(user, field.GetValue(data));
                //        }
                //    }
                //}
                // Между делом жаловался на нулл дереференс, ну а мне-то шо?

                db.SaveChanges();
            });
            
            if (result.Length > 0) return Conflict(result);
            return Ok();
        }

        string MakeHash(string password)
        {
            return password; //TODO hash-функцию завезём позже. И соль в структуру БД... Хотя смысла в этом вижу мало.
        }
    }
}