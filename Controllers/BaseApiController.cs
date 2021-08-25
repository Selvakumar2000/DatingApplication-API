using DatingApp.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))] //used for update LastActive property
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController : ControllerBase
    {

    }
}
