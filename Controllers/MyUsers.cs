using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.Data;
using DatingApp.DTOs;
using DatingApp.Entities;
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
    public class MyUsers : ControllerBase
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public MyUsers(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{username}")]
        public async Task<MemberDto> GetUsers(string username)
        {
            var users = await _context.Users
                   .Where(x => x.UserName == username)
                   .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //Automapper Queryable Extension
                   .SingleOrDefaultAsync();
            return users;
           
        }
    }
}
