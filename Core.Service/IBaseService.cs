using Core.Services.Locator;
using Furion.DatabaseAccessor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public interface IBaseService<T> : IDbSelector<T> where T : class, IPrivateEntity, new()
    {
        /// <summary>
        /// 根据条件predicate，获取数据库内容
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<List<T>> GetWithCondition(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件predicate，获取数据库内容（不跟踪）
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<List<T>> GetWithConditionNt(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件predicate，获取数据库内容（不跟踪），懒查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetQueryableNt(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 根据条件predicate，获取数据库内容,懒查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 获取所有数据，获取数据库内容,懒查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetQueryable();
        /// <summary>
        /// 获取所有数据，获取数据库内容（不跟踪）,懒查询
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> GetQueryableNt();
        /// <summary>
        /// 根据条件predicate,判断数据库是否存在某条数据
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<bool> Exist(Expression<Func<T, bool>> predicate);
        /// <summary>
        /// 添加一条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<T> Add(T entity);
        /// <summary>
        /// 添加多条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task Add(ICollection<T> entity);
        /// <summary>
        /// 立即添加一条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task<T> AddNow(T entity);
        /// <summary>
        /// 立即添加多条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task AddNow(ICollection<T> entity);
        /// <summary>
        /// 更新一条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task Update(T entity);
        /// <summary>
        /// 立即更新一条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task UpdateNow(T entity);
        /// <summary>
        /// 更新多条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task Update(ICollection<T> entity);
        /// <summary>
        /// 立即更新多条数据到数据库(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task UpdateNow(ICollection<T> entity);
        /// <summary>
        /// 从数据库删除一条数据(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task Delete(T entity);
        /// <summary>
        /// 从数据库删除多条数据(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task Delete(ICollection<T> entity);
        /// <summary>
        /// 立即从数据库删除一条数据(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task DeleteNow(T entity);
        /// <summary>
        /// 立即从数据库删除多条数据(Insert)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task DeleteNow(ICollection<T> entity);
        /// <summary>
        /// 保存数据，如果不是调用立即更新/添加/删除，应手动调用这条方法
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Task SaveChangeNow();
        /// <summary>
        /// 获取当前service租户信息
        /// </summary>
        /// <returns></returns>
        public string GetTenant();
        public void SetTenant(string tenantId);

    }
}
