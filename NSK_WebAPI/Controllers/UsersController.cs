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
        public ActionResult<IEnumerable<User>> GetUsers([FromHeader(Name="Token")] string tokenString)
        {
            return DatabaseContext.ExecuteAndReturn((db, _) => new ActionResult<IEnumerable<User>>(db.Users), tokenString, "ReadOthers");
        }
        
        [HttpGet, Route("GetUser/{userId}")]
        public ActionResult<User> GetUser([FromRoute(Name="userId")]int userId, [FromHeader(Name="Token")] string tokenString)
        {
            var foundUser = DatabaseContext.ExecuteAndReturn((db, token) =>
            {
                if (!Permissions.CanGetUser(db, userId, token)) return new ForbidResult();
                return new ActionResult<User>(db.Users.FirstOrDefault(u => u.UserId == userId));
            }, tokenString);
            
            Console.WriteLine("foundUser is "+foundUser+", its result is "+foundUser.Result+", its value is "+foundUser.Value);
            if (foundUser.Result is not null) return foundUser.Result;
            if(foundUser.Value is null)
            {
                Console.WriteLine("foundUser is null");
                return new ActionResult<User>(new NotFoundResult());
            }
            Console.WriteLine("returning it");
            
            return foundUser;
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
        [HttpPost, Route("LoginUser")]
        public ActionResult<string> LoginUser([FromBody]RequestLoginByPassword data)
        {
            var tokenString = DatabaseContext.ExecuteAndReturn(db =>
            {
                User? user = null;
                if(data.Phone!=null)user = db.Users.FirstOrDefault(u => u.PhoneNumber == data.Phone && u.PassHash == MakeHash(data.Password));
                else if(data.Email!=null)user = db.Users.FirstOrDefault(u => u.Email == data.Email && u.PassHash == MakeHash(data.Password));
                //Сверху лучше не скипать, т.к. при теоретическом существовании юзера без того и другого соответственный запрос залогинит за него
                if (user is null) return null;
                return db.Tokens.FirstOrDefault(t => t.UserId == user.UserId)?.TokenString;
            });
            if (tokenString is null) return new NotFoundResult();
            return new ActionResult<string>(tokenString);
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

        public static string MakeHash(string password)
        {
            return password; //TODO hash-функцию завезём позже. И соль в структуру БД... Хотя смысла в этом вижу мало.
        }
    }
}