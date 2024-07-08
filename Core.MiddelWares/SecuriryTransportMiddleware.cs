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


    public static class RSAEncryptionExtension
    {
        /// <summary>
        /// 签名
        /// </summary>
        /// <param name="halg">算法如:SHA256</param>
        /// <param name="text">明文内容</param>
        /// <param name="privateKey">私钥</param>
        /// <returns>签名内容</returns>
        public static string Sign(string halg, string text, string privateKey)
        {
            string encryptedContent;
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(privateKey);

            var encryptedData = rsa.SignData(Encoding.Default.GetBytes(text), halg);
            encryptedContent = Convert.ToBase64String(encryptedData);

            return encryptedContent;
        }

        /// <summary>
        /// 校验
        /// </summary>
        /// <param name="halg">算法如:SHA256</param>
        /// <param name="text">明文内容</param>
        /// <param name="publicKey">公钥</param>
        /// <param name="sign">签名内容</param>
        /// <returns>是否一致</returns>
        public static bool Verify(string halg, string text, string publicKey, string sign)
        {
            using var rsa = new System.Security.Cryptography.RSACryptoServiceProvider();
            rsa.FromXmlString(publicKey);

            return rsa.VerifyData(Encoding.Default.GetBytes(text), halg, Convert.FromBase64String(sign));
        }
    
    }
}
