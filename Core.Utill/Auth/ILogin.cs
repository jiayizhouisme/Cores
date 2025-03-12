using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth
{
    public interface ILogin
    {
        public Task<string> Login();
        public string Token { get; }
    }
}
