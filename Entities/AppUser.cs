using DatingApp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
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
        public ICollection<Photo> Photos { get; set; }  //one to many relationship (one user can have many photos)

        //list of users that like the currently loggedin user (received likes)
        public ICollection<UserLike> LikedByUsers { get; set; }

        //list of users that are the currently loggedin user has liked (give likes)
        public ICollection<UserLike> LikedUsers { get; set; }

        public ICollection<Message> MessageSent { get; set; }
        public ICollection<Message> MessageReceived { get; set; }

    }
}