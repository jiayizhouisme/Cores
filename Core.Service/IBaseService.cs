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
        public Task<List<T>> GetWithCondition(Expression<Func<T, bool>> predicate);
        public Task<List<T>> GetWithConditionNt(Expression<Func<T, bool>> predicate);
        public IQueryable<T> GetQueryableNt(Expression<Func<T, bool>> predicate);
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate);
        public Task<bool> Exist(Expression<Func<T, bool>> predicate);
        public Task<T> Add(T entity);
        public Task Add(ICollection<T> entity);
        public Task<T> AddNow(T entity);
        public Task AddNow(ICollection<T> entity);
        public Task Update(T entity);
        public Task UpdateNow(T entity);
        public Task Update(ICollection<T> entity);
        public Task UpdateNow(ICollection<T> entity);
        public Task Delete(T entity);
        public Task Delete(ICollection<T> entity);
        public Task DeleteNow(T entity);
        public Task DeleteNow(ICollection<T> entity);
        public IRepository<T> GetRepository();

    }
}
