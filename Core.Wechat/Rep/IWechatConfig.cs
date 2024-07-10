using Core.Wechat.Entity;
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
    }
}
