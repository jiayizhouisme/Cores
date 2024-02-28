using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SignalR
{
    public interface ISignalRUserService
    {
        public void AddClient(string userId, RealOnlineClient client);

        public void RemoveClientByUserId(string userId);
        public void RemoveClientByConnId(string connId);

        public RealOnlineClient isOnline(string userId);
      
    }
}
