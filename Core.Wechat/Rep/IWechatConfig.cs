using Core.Wechat.Entity;
using Core.Wechat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Rep
{
    public interface IWechatConfig
    {
        public Task<ICollection<WechatConfigBase>> GetConfigs();
        public Task<WechatUser?> GetWechatUserByOpenId(string openid);
        public Task<WechatUser?> InsertUserToRepo(WechatUser wechatUser);
        public Task<WechatUser?> UpdateUserToRepo(WechatUser wechatUser);
    }
}
