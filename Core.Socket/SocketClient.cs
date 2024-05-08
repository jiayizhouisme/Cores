using System.Net.Sockets;
using TouchSocket.Core;
using TouchSocket.Sockets;
using Furion;
using TcpClient = TouchSocket.Sockets.TcpClient;
using Furion.JsonSerialization;
using System.Net.Http;
using Furion.EventBus;
using BeetleX.Buffers;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Furion.DependencyInjection;
using Furion.Logging;
using Microsoft.Extensions.Logging;

namespace Core.Socket
{
    public class SocketClient : ISingleton
    {

        public string ip { get; set; }
        public string port { get; set; }
        protected TcpClient tc;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SocketClient> logger;

        public SocketClient(IEventPublisher _eventPublisher, ILogger<SocketClient> logger)
        {
            this._eventPublisher = _eventPublisher;
            this.tc = new TcpClient();
            this.logger = logger;
        }

        protected virtual void init()
        {
            tc.Connecting = (client, e) => { return EasyTask.CompletedTask; };//即将连接到服务器，此时已经创建socket，但是还未建立tcp
            tc.Connected = (client, e) => { 
                
                return EasyTask.CompletedTask; };//成功连接到服务器
            tc.Disconnecting = (client, e) => { return EasyTask.CompletedTask; };//即将从服务器断开连接。此处仅主动断开才有效。
            tc.Disconnected = (client, e) => { return EasyTask.CompletedTask; };//从服务器断开连接，当连接不成功时不会触发。
            tc.Received = async (client, e) =>
            {
                logger.LogInformation("Socket Received count:" + e.Count);
                byte[] b = new byte[e.ByteBlock.Len];
                await e.ByteBlock.ReadAsync(b, 0, b.Length);
                await _eventPublisher.PublishAsync("SocketReceived",b);

                await EasyTask.CompletedTask;
            };

        }
        public virtual async Task start()
        {
            this.init();
            tc.Setup(new TouchSocketConfig()
                .SetRemoteIPHost(ip + ":" + port)
                .ConfigurePlugins(a => {
                    a.UseReconnection(-1, true, -1)
                    .SetTick(TimeSpan.FromSeconds(1))
                    .UsePolling();
                })
                .ConfigureContainer(a =>
                {
                    a.AddConsoleLogger();//添加一个日志注入
                    
                }));

            tc.Connect();//调用连接，当连接不成功时，会抛出异常。
        }
    }
}
