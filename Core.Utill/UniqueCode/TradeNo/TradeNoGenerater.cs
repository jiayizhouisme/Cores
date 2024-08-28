using Core.Cache;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utill.UniqueCode
{
    public class TradeNoGenerater : ITradeNoGenerater<long>,ISingleton
    {
        private readonly IUniqueCodeGenerater<long> codeGenerator;
        public TradeNoGenerater(IUniqueCodeGenerater<long> codeGenerator)
        {
            this.codeGenerator = codeGenerator;
        }
        public async Task<long> Generate()
        {
            DateTime currentTime = DateTime.Now;
            string originDateStr = currentTime.ToString("yyMMdd");
            long differSecond = currentTime.Minute * 60 + currentTime.Second;

            string yyMMddSecond = originDateStr + differSecond.ToString().PadLeft(5, '0');
            var parsed = long.Parse(yyMMddSecond);

            var res = await codeGenerator.Generate("TradeNo");

            //生成订单编号
            string orderNo = parsed.ToString() + res.ToString().PadLeft(4, '0');
            return long.Parse(orderNo);
        }
    }
}
