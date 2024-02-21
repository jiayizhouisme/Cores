using Core.Auth;
using Furion.InstantMessaging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.SignalR
{
    [Authorize]
    [MapHub("/hubs/chathub")]
    public class ChatHub : Hub<IChatClient>
    {
        private readonly ISignalRUserService userapp;
        private readonly IHttpContextUser httpContextUser;
        public ChatHub(ISignalRUserService userapp, IHttpContextUser httpContextUser) : base()
        {
            this.userapp = userapp;
            this.httpContextUser = httpContextUser;
        }

        public async override Task OnConnectedAsync()
        {
            var connId = Context.ConnectionId;
            var name = httpContextUser.ID;
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            var real = userapp.isOnline(name);

            var client = new RealOnlineClient
            {
                ConnId = connId,
                UserId = name,
                ConnectServerTime = DateTime.Now
            };
            if (real == null)
                userapp.AddClient(name, client); //新增
            else
            {
                //1、移除
                userapp.RemoveClient(real.UserId);
                //2、新增
                userapp.AddClient(name, client);
            }
            await base.OnConnectedAsync();
            await Task.CompletedTask;
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var connId = Context.ConnectionId;
            userapp.RemoveClient(connId);
            await base.OnDisconnectedAsync(exception);
        }

    }

}
