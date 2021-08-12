using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using DatingApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService _tokenService;
        public AccountController(DataContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        //post data using query parameter
        //[HttpPost]
        ////api/account
        //public async Task<ActionResult<AppUser>> Register(string username,string password)
        //{
        //    using var hmac = new HMACSHA512();
        //    var user = new AppUser
        //    {
        //        UserName = username,
        //        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password)),
        //        PasswordSalt = hmac.Key
        //    };
        //    _context.Users.Add(user);
        //    await _context.SaveChangesAsync();

        //    return user;
        //}  

        //post data using from JSON objects
        [HttpPost("Register")]
        //api/account
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.Username)) return BadRequest("UserName is already taken");
  
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username,
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
        //make username to be unique
        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName==username);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            /*UserName Check*/
            var user = await _context.Users.SingleOrDefaultAsync(x=>x.UserName==loginDto.Username);
            if (user == null) return Unauthorized("Invalid UserName");

            /*Password Check
 Reference for Decryption:https://ourtechroom.com/tech/password-hashing-using-hmacsha512-asp.net-application/            
             */
            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));
            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            } 
            return new UserDto
            {
                Username = user.UserName,
                Token = _tokenService.CreateToken(user)
            };
        }
    }
}
