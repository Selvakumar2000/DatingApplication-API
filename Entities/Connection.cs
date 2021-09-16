using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.Entities
{
    public class Connection
    {
        public Connection()
        {
            //default constructor is for EF, when it creates a table it needs an empty constructor
        }

        public Connection(string connectionId, string username)
        {
            ConnectionId = connectionId;
            Username = username;
        }

        public string ConnectionId { get; set; }  //ClassNameId ->EF take this as primary key
        public string Username { get; set; }
    }
}
