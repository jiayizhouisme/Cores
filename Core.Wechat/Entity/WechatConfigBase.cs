using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Entity
{
    public abstract class WechatConfigBase
    {
        public string key { get; set; }
        public string appid { get; set; }
        public string appSecret { get; set; }
    }
}
