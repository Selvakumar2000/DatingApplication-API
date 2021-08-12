using DatingApp.Data;
using DatingApp.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        //Inject DataContext here in order to access,insert and manupulate the database 
        private readonly DataContext _context;
        public UsersController(DataContext context)
        {
            _context = context;
        }

        //[HttpGet]
        ////api/users
        //public ActionResult<IEnumerable<AppUser>> GetUsers()
        //{
        //    var users = _context.Users.ToList();
        //    return users;
        //}

        //[HttpGet("{id}")]
        ////api/users/2
        //public ActionResult<AppUser> GetUsers(int id)
        //{
        //    var user = _context.Users.Find(id);
        //    return user;
        //}

        //make this all methods as aynchronous methods
        [HttpGet]
        [AllowAnonymous]
        //api/users
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpGet("{userid}")]
        [Authorize]
        //api/users/2
        public async Task<ActionResult<AppUser>> GetUsers(int userid)
        {
            var user = await _context.Users.FindAsync(userid);
            return user;
        }
    }
}
