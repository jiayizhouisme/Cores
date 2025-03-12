using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Core.Auth.Model;
using Newtonsoft.Json;

namespace Core.Auth
{
    public class LoginImpl : ILogin
    {
        private long exp = 0;
        private string token { get; set; }
        public string Token => token;

        public async Task<string> Login()
        {
            if (!IsExpired())
            {
                return this.token;
            }

            HttpClient hc = new HttpClient();
            HttpContent content = new StringContent(JsonConvert.SerializeObject(new LoginModel()
            {
                username = "staff1",
                password = "Aa123456"

            }));
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");
            var request = await hc.PostAsync("https://ticket.z2ww.com/shibabang/api/Login/Login", content).ConfigureAwait(false);

            IEnumerable<string> token;

            request.Headers.TryGetValues("access-token", out token);
                
            if (token != null)
            {
                this.token = token.FirstOrDefault();
            }

            if (this.token != null)
            {
                JwtTokenDecode(this.token);
            }
            return this.token;

        }

        private bool IsExpired()
        {
            var timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds();
            var sub = exp - timestamp;
            if (sub <= 360)
            {
                return true;
            }
            return false;
        }


        private void JwtTokenDecode(string token)
        {

            string[] split = token.Split('.');
            string payload = split[1];

            string padded = payload.Length % 4 == 0
                ? payload : payload + "====".Substring(payload.Length % 4);
            string base64 = padded.Replace("_", "/")
                .Replace("-", "+");

            byte[] outputb = Convert.FromBase64String(base64);
            var orgStr = JsonConvert.DeserializeObject<dynamic>(Encoding.Default.GetString(outputb));
            this.exp = orgStr.exp;
        }
    }

    
}
