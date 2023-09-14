using Microsoft.AspNetCore.Mvc;
using ZubrServer.DB;
using ZubrServer.DB.DBObjects;

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
        
        private DatabaseContext db = new(); //Нужно протестить на лайфтайм, почему бы не юзать один контекст для всех запросов? Многопоточность... Конечно же.

        [HttpGet(Name = "GetUsers")]
        public IEnumerable<User> Get()
        {
            return db.Users;
        }
    }
}