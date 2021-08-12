﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.DTOs
{
    public class RegisterDto
    {
        [Required]
        public string Username { get; set; }  //we can easily map this with AppUser UserName property.Used for assigning
        
        [Required]
        [StringLength(8,MinimumLength =4)]
        public string Password { get; set; }
    }
}