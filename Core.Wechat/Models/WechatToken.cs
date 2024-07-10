using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Models
{
    public struct WechatToken
    {
        public string AccessToken { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
