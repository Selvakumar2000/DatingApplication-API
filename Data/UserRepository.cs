﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using DatingApp.DTOs;
using DatingApp.Entities;
using DatingApp.Helpers;
using DatingApp.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext context,IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                   .Where(x => x.UserName == username)
                   //.Select(user => new MemberDto //it return all the properties in our db
                   //{
                   //    Id=user.Id,
                   //    Username=user.UserName //if we have 20,30 properties,this won't be a good choice
                   //})
                   .ProjectTo<MemberDto>(_mapper.ConfigurationProvider) //Automapper Queryable Extension
                   .SingleOrDefaultAsync();
        }

        public async Task<PagedList<MemberDto>> GetMembersAsync(UserParams userParams)
        {
            //filter the user before sending the memberdto
            var query = _context.Users.AsQueryable();
            query = query.Where(u => u.UserName != userParams.CurrentUsername);
            query = query.Where(u => u.Gender == userParams.Gender);

            var minDob = DateTime.Today.AddYears(-userParams.MaxAge - 1);
            var maxDob = DateTime.Today.AddYears(-userParams.MinAge);

            query = query.Where(u => u.DateOfBirth >= minDob && u.DateOfBirth <= maxDob);

            //sort
            query = userParams.OrderBy switch
            {
                "created" => query.OrderByDescending(u => u.Created),  //for case:created
                _ => query.OrderByDescending(u => u.LastActive)        //for case:default
            };

            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
                                                               .AsNoTracking(),userParams.PageNumber, userParams.PageSize);
                   
        }

        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            return await _context.Users.FindAsync(id); 

        }
        public async Task<AppUser> GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                .Include(p=>p.Photos)
                .SingleOrDefaultAsync(x => x.UserName == username);
        }
        public async Task<IEnumerable<AppUser>> GetUsersAsync()
        {
            return await _context.Users
                .Include(p => p.Photos)   //to get data from photos table,but it cause circular dependency error 
                .ToListAsync(); //to avoid this,use a dto,in which what data you want to return
        }                       //because photo class have appuser property and appuser have photos property
        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }
        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

    }
}
