using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class AppUserRole : IdentityUserRole<int>
    {
        //specify the joint entities that we need
        public AppUser User { get; set; }
        public AppRole Role { get; set; }
    }
}
