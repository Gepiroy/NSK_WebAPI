using Microsoft.AspNetCore.Mvc;
using NSK_WebAPI.DB;
using NSK_WebAPI.DB.DBObjects;

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

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> GetUsers() // Охренеть ты коллекционер
        {
            var db = DatabaseContext.LockContext();
            var users = db.Users;
            DatabaseContext.ReleaseContext();

            return users;
        }
        
        [HttpGet, Route("GetUser/{userid}")]
        public User GetUser(int userid)
        {
            var db = DatabaseContext.LockContext();
            var user = db.Users.First(u => u.UserId==userid);
            DatabaseContext.ReleaseContext();

            return user;
        }
    }
}