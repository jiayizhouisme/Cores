using System.ComponentModel.DataAnnotations;

namespace Core.User.Entity
{
    public abstract class UserBase
    {
        public string username { get; set; }
        public string password { get; set; }
    }
}
