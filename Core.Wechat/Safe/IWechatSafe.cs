using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Safe
{
    public interface IWechatSafe
    {
        public System.Threading.Tasks.Task BeginGetWechatUser(string OpenId);
        public System.Threading.Tasks.Task EndGetWechatUser(string OpenId);
        public System.Threading.Tasks.Task BeginCheckAccessToken(string AppId);
        public System.Threading.Tasks.Task EndCheckAccessToken(string AppId);
    }
}
