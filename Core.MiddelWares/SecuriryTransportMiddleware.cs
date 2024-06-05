using Furion.DataEncryption;
using Furion.JsonSerialization;
using Furion.LinqBuilder;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.MiddelWares
{
    public class SecuriryTransportMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IJsonSerializerProvider jsonProvider;
        private static (string, string) keys;

        public SecuriryTransportMiddleware(RequestDelegate next, IJsonSerializerProvider jsonProvider)
        {
            _next = next;
            this.jsonProvider = jsonProvider;
            if (keys.Item1.IsNullOrEmpty() || keys.Item2.IsNullOrEmpty()) {
                keys = RSAEncryption.GenerateSecretKey(2048);
                var en = RSAEncryption.Encrypt(jsonProvider.Serialize(jsonProvider.Deserialize<object>("{\r\n  \"username\": \"string\",\r\n  \"password\": \"string\"\r\n}"))
                    , keys.Item1);
            }
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // request handle
            if (context.Request.Method != HttpMethods.Post ||
                !context.Request.Path.ToString().Contains("api"))
            {
                await _next(context);
                await Task.CompletedTask;
            }
            else
            {
                var encryptedBody = context.Request.Body;
                var encryptedContent = await new StreamReader(encryptedBody).ReadToEndAsync();
                var decryptedBody = RSAEncryption.Decrypt(encryptedContent,keys.Item2);
                var requestContent = new StringContent(decryptedBody, Encoding.UTF8, "application/json");
                var stream = await requestContent.ReadAsStreamAsync();
                context.Request.Body = stream;

                await _next(context);
                // response handle
                var originContent = await new StreamReader(context.Response.Body).ReadToEndAsync();
                var encryptedBodyStr = RSAEncryption.Encrypt(originContent,keys.Item1);
                var responseContent = new StringContent(encryptedBodyStr, Encoding.UTF8, "application/json");
                context.Response.Body = await responseContent.ReadAsStreamAsync();
            }
            
            
            // 或者直接
            // await context.Response.WriteAsync(encryptedBody);
        }
    }

    /// <summary>
    /// 中间件扩展帮助类
    /// </summary>
    public static class SecuriryTransportMiddlewareExtensions
    {
        /// <summary>
        /// 请求上下文中间件
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseSecuriryTransportMildd(this IApplicationBuilder app)
        {
            return app.UseMiddleware<SecuriryTransportMiddleware>();
        }
    }
}
