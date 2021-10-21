using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.SignalR
{
    public class PresenceTracker
    {
/*   ConnectionId:
 *Everytime the user connected to the hub, they are going to be given a connectionId. Now there is nothing to stop a user from
 connecting to the same application from a different device and they would get a different connectionId for each different
 connection that they are having or making to our application.
 */
//store username and connectionId(list of string) as a key-value pair and it is shared among every user who connect to the server.
        private static readonly Dictionary<string, List<string>> OnlineUsers = new Dictionary<string, List<string>>();

        public Task UserConnected(string username, string connectionId)
        {
            lock(OnlineUsers)
            {
                if(OnlineUsers.ContainsKey(username))
                {
                    //if the user already in a dictionary, adding connectionId to that user
                    OnlineUsers[username].Add(connectionId);
                }
                else
                {
                    //if the user is newly connected to the server, add them with username and connectionId inside the dictionary.
                    OnlineUsers.Add(username, new List<string> { connectionId });
                }
            }
            return Task.CompletedTask;
        }

        public Task UserDisconnected(string username, string connectionId)
        {
            lock(OnlineUsers)
            {
                if (!OnlineUsers.ContainsKey(username)) return Task.CompletedTask;

                OnlineUsers[username].Remove(connectionId);

                if(OnlineUsers[username].Count == 0)
                {
                    OnlineUsers.Remove(username);
                }
            }
            return Task.CompletedTask;
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] onlineUsers;
            
            lock(OnlineUsers)
            {
                onlineUsers = OnlineUsers.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }
            return Task.FromResult(onlineUsers);
        }

        public Task<List<string>> GetConnectionsForUser(string username)
        {
            List<string> connectionIds;

            lock (OnlineUsers)
            {
                connectionIds = OnlineUsers.GetValueOrDefault(username);
            }

            return Task.FromResult(connectionIds);
        }
    }
}
