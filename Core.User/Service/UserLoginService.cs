using Core.Services;
using Core.User.Entity;
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
    }
}
