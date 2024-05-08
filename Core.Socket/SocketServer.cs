using Furion.DependencyInjection;
using Furion.EventBus;
using Microsoft.Extensions.Logging;
using ProtoBuf.Meta;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;
using TouchSocket.Sockets;

namespace Core.Socket
{
    public class SocketServer : ISingleton
    {
        protected TcpService tc;
        private readonly IEventPublisher _eventPublisher;
        private readonly ILogger<SocketClient> logger;

        public SocketServer(IEventPublisher _eventPublisher, ILogger<SocketClient> logger)
        {
            this._eventPublisher = _eventPublisher;
            this.tc = new TcpService();
            this.logger = logger;
        }

        protected virtual void init()
        {
            tc.Connecting = (client, e) => { return EasyTask.CompletedTask; };//即将连接到服务器，此时已经创建socket，但是还未建立tcp
            tc.Connected = (client, e) => {

                return EasyTask.CompletedTask;
            };//成功连接到服务器
            tc.Disconnecting = (client, e) => { return EasyTask.CompletedTask; };//即将从服务器断开连接。此处仅主动断开才有效。
            tc.Disconnected = (client, e) => { return EasyTask.CompletedTask; };//从服务器断开连接，当连接不成功时不会触发。
            tc.Received = async (client, e) =>
            {
                logger.LogInformation("Socket Received count:" + e.ByteBlock.Len);
                byte[] b = new byte[e.ByteBlock.Len];
                await e.ByteBlock.ReadAsync(b, 0, b.Length);
                await _eventPublisher.PublishAsync("SocketReceived", b);

                await EasyTask.CompletedTask;
            };

        }

        public virtual async Task start()
        {
            this.init();
            tc.Setup(new TouchSocketConfig()//载入配置
            .SetListenIPHosts(7790)//同时监听两个地址
            .ConfigureContainer(a =>//容器的配置顺序应该在最前面
            {
                a.AddConsoleLogger();//添加一个控制台日志注入（注意：在maui中控制台日志不可用）
            })
            .ConfigurePlugins(a =>
            {
                //a.Add();//此处可以添加插件
            }));

            tc.Start();//启动
        }
    }
}
