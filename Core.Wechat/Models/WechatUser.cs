using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.Models
{
    public struct WechatUser
    {
        public string Key { get; set; }
        public string OpenID { get; set; }
        public string UnionID { get; set; }
        public string Access_Token { get; set; }
        public string Refresh_Token { get; set; }
        public int Expire_in { get; set; }
        public string HeadImg { get; set; }
        public string NickName { get; set; }
        public string Province { get; set; }
        public string City { get; set; }    
        public string Country { get; set; }
        public int? Sex { get; set; }

    }
}
