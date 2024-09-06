using Core.Services;
using Core.User.Entity;
using Core.User.Model;
using Furion.DatabaseAccessor;
using Furion.DataEncryption.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.User.Service
{
    public abstract class UserLoginService<T,DbLocator> : BaseService<T, DbLocator>, IUserLoginService<T> where T : UserBase, IPrivateEntity, new() where DbLocator : class,IDbContextLocator
    {
        public UserLoginService(IRepository<T, DbLocator> repository)
        {
            this._dal = repository;
        }
        public async Task<T> Login(T user)
        {
            string _password = null;
            T _user = null;
            user.password = user.password.ToMD5Encrypt();

            if (_password == null)
            {
                _user = await this._dal.Where(a => a.username == user.username && a.password == user.password).FirstOrDefaultAsync();
                if (_user == null)
                {
                    return null;
                }
                return _user;
            }
            else if (_password == user.password)
            {
                return _user;
            }
            return null;
        }

        public async Task<ChangePasswordRetModel> UpdatePassword(string userName, ChangePasswordModel model)
        {
            ChangePasswordRetModel ret = new ChangePasswordRetModel {code = 0 };
            var oldPassword = model.oldPassword.ToMD5Encrypt();
            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(oldPassword) && !string.IsNullOrEmpty(model.newPassword))
            {
                var user = await this._dal.AsQueryable(a => a.username == userName && a.password == oldPassword).FirstOrDefaultAsync();
                if (user != null)
                {
                    user.password = model.newPassword.ToMD5Encrypt();
                    await this._dal.UpdateNowAsync(user);
                    ret.message = "密码修改成功" ;
                    ret.code = 1;
                }
                ret.message = "旧密码与原密码不一致";
                ret.code = 0;
            }
            return ret;
            
        }
    }
}
