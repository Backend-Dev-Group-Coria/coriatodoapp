using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CoriaToDo.API.Feature.Health;
[Route("api/[controller]")]
[ApiController]
public class HeartBeatController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "It's alive!";
    }
}
