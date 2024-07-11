using Core.Wechat.Models;
using Core.Wechat.Rep;
using Furion.DependencyInjection;
using Furion.EventBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Wechat.EventBus
{
    public class WechatUserCheckAndUpdateEvent : IEventSubscriber, ISingleton
    {
        private readonly IWechatConfig _config;
        private readonly IWechat wechat;
        public static string Event_OnGetWechatUserInfoFromWechatFirstTime = "OnGetWechatUserInfoFromWechatFirstTime";
        public static string Event_OnGetWechatUserInfoFromCustom = "OnGetWechatUserInfoFromCustom";
        //首次查找
        [EventSubscribe("OnGetWechatUserInfoFromWechatFirstTime")]
        public async System.Threading.Tasks.Task OnGetWechatUserInfoFromWechatFirstTime(EventHandlerExecutingContext context)
        {
            var todo = context.Source;
            var data = (WechatUser)todo.Payload;

            await _config.InsertUserToRepo(data);
        }

        //比对仓储的数据和微信线上是否一致
        [EventSubscribe("OnGetWechatUserInfoFromCustom")]
        public async System.Threading.Tasks.Task OnGetWechatUserInfoFromCustom(EventHandlerExecutingContext context)
        {
            var todo = context.Source;
            var data = (WechatUser)todo.Payload;

            //从微信查找
            var entity = await wechat.GetUserInfoByOpenID(data.Key,data.OpenID,data.Access_Token);
            if (entity != null)
            {
                if (data.NickName != entity.Value.NickName
                    || data.HeadImg != entity.Value.HeadImg )
                {
                    //发现不一致的地方，开始更新仓储
                    await _config.UpdateUserToRepo(entity.Value);
                }
                
            }
        }
    }
}
