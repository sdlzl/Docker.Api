using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private ILogger<UserController> _logger;
        private UserContext _userContext;
        public UserController(ILogger<UserController> logger,UserContext userContext)
        {
            _logger = logger;
            _userContext = userContext;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var users = await _userContext.Users.ToListAsync();
            return new JsonResult(users);
        }
    }
}
