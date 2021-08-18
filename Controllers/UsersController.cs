using AutoMapper;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DatingApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [Authorize] //all the methods inside this controller will be protected with authorization
    public class UsersController : ControllerBase
    {
        //Inject DataContext here in order to access,insert and manupulate the database 
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        public UsersController(IUserRepository userRepository,IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        [HttpGet]
        //api/users
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            var users = await _userRepository.GetMembersAsync();
            return Ok(users);
        }

        [HttpGet("{username}")]
        //api/users/name
        public async Task<ActionResult<MemberDto>> GetUserByUsername(string username)
        {
            return  await _userRepository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;//return user's UserName from the token that the API uses to authenticate (claims)
            var user = await _userRepository.GetUserByUsernameAsync(username);

            _mapper.Map(memberUpdateDto, user);  //or user.City=memberUpdateDto.City like this we need to write.

//if we update the same information again,we are not going to get an error
            _userRepository.Update(user);

            //update the fileds
            if (await _userRepository.SaveAllAsync()) return NoContent();
            return BadRequest("Fail to Update User");

        }
    }
}
