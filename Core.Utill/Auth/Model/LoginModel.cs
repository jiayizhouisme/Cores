using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Auth.Model
{
    public class LoginModel
    {
        public string username { get; set; }
        public string password { get; set; }
        public string device_name { get; set; }
    }
}
