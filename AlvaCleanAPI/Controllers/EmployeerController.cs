using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "employeer,admin")]
    public class EmployeerController : Controller
    {
        
    }
}
