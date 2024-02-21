using System.ComponentModel.DataAnnotations;

namespace Core.User.Entity
{
    public abstract class UserBase
    {
        [Required, Display(Name = "用户姓名")]
        public string username { get; set; }
        [Required, Display(Name = "用户密码")]
        public string password { get; set; }
    }
}
