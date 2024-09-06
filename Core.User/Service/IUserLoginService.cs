using Core.Services;
using Core.User.Entity;
using Core.User.Model;
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public interface IUserLoginService<T> where T : UserBase, IPrivateEntity, new()
    {
        Task<T> Login(T user);
        Task<ChangePasswordRetModel> UpdatePassword(string userName,ChangePasswordModel model);
    }
}
