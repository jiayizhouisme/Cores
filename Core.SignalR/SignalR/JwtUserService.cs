using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
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
            users = new Dictionary<string, RealOnlineClient>();
        }

        public void AddClient(string connId, RealOnlineClient client)
        {
            users.Add(connId, client);
        }

        public void RemoveClient(string connId)
        {
            users.Remove(connId);
        }

        public RealOnlineClient isOnline(string userId)
        {
            var client = users.Where(a => a.Value != null && a.Value.UserId == userId).Select(a => a.Value);
            if (client.Count() > 0)
            {
                return client.FirstOrDefault();
            }
            return null;
        }
    }

    public class RealOnlineClient
    {
        public string ConnId { get; set; }
        public string UserId { get; set; }
        public DateTime ConnectServerTime { get; set; }
    }
}
