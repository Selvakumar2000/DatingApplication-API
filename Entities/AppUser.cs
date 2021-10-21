﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class AppUser : IdentityUser<int>
    {
        public DateTime DateOfBirth { get; set; }
        public string KnownAs { get; set; }
        public DateTime Created { get; set; } = DateTime.Now;
        public DateTime LastActive { get; set; } = DateTime.Now;
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }
        public string Interests { get; set; }
        public string City { get; set; }
        public string Country { get; set; }

        //one to many relationship (one user can have many photos)
        public ICollection<Photo> Photos { get; set; }

        //many to one relationship
        public ICollection<UserLike> LikedByUsers { get; set; }//list of users that like the currently loggedin user(received likes)

        //list of users that are the currently loggedin user has liked (give likes)
        public ICollection<UserLike> LikedUsers { get; set; }

        //many to many relationship
        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }

        public ICollection<AppUserRole> UserRoles { get; set; }

    }
}