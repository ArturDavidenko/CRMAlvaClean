using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AlvaCleanAPI.Controllers
{

    [ApiController]
    [Route("[controller]")]
    [Authorize(Roles = "employeer")]
    [Authorize(Roles = "admin")]
    public class EmployeerController : Controller
    {
        
    }
}
