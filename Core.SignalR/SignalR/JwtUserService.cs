using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.SignalR
{
    public class JwtUserService : ISignalRUserService
    {
        private IDictionary<string, RealOnlineClient> users;

        public JwtUserService()
        {
            users = new ConcurrentDictionary<string, RealOnlineClient>();
        }

        public void AddClient(string userId, RealOnlineClient client)
        {
            users.Add(userId, client);
        }

        public void RemoveClientByUserId(string userId)
        {
            RealOnlineClient client = null;
            users.TryGetValue(userId, out client);
            if (client != null)
            {
                users.Remove(userId);
            }
            
        }

        public void RemoveClientByConnId(string connId)
        {
            var client = this.users.Where(a => a.Value != null && a.Value.ConnId == connId).Select(a => a.Key).FirstOrDefault();
            if (client != null)
            {
                this.RemoveClientByUserId(client);
            }

        }

        public RealOnlineClient isOnline(string userId)
        {
            RealOnlineClient client = null;
            users.TryGetValue(userId, out client);
            return client;
        }          
    }

    public class RealOnlineClient
    {
        public string ConnId { get; set; }
        public string UserId { get; set; }
        public DateTime ConnectServerTime { get; set; }
    }
}
