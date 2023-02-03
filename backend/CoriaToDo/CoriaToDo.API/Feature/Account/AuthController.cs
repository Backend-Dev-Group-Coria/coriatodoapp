using CoriaToDo.API.Data;
using CoriaToDo.API.Feature.Todo.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoriaToDo.API.Feature.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly SessionContext _sessionContext;

        public AuthController(SessionContext sessionContext)
        {
            _sessionContext = sessionContext;
        }

        [HttpPost]
        [Route("login/{id}")]
        public IActionResult Login(int id)
        {
            _sessionContext.UserId = id;
            return Ok();
        }
    }
}
