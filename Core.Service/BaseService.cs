﻿using Furion.DatabaseAccessor;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class BaseService<T,DbLocator> : IBaseService<T> where T : class, IPrivateEntity, new() where DbLocator : class,IDbContextLocator
    {
        protected IRepository<T, DbLocator> _dal;
        private string _connStr;

        public async Task<T> Add(T entity)
        {
            var result = await _dal.InsertAsync(entity);
            return result.Entity;
        }

        public async Task Add(ICollection<T> entities)
        {
            await _dal.InsertAsync(entities);
        }

        public async Task<T> AddNow(T entity)
        {
            var result = await _dal.InsertNowAsync(entity);
            return result.Entity;
        }

        public async Task AddNow(ICollection<T> entities)
        {
            await _dal.InsertNowAsync(entities);
        }

        public async Task Delete(T entity)
        {
            var result = await _dal.DeleteAsync(entity);
        }

        public async Task Delete(ICollection<T> entity)
        {
            await _dal.DeleteAsync(entity);
        }

        public async Task DeleteNow(T entity)
        {
            await _dal.DeleteNowAsync(entity);
        }

        public async Task DeleteNow(ICollection<T> entity)
        {
            await _dal.DeleteNowAsync(entity);
        }

        public IQueryable<T> GetQueryableNt(Expression<Func<T, bool>> predicate)
        {
            return _dal.AsQueryable().AsNoTracking().Where(predicate);
        }

        public IQueryable<T> GetQueryable()
        {
            return _dal.AsQueryable();
        }
        public IQueryable<T> GetQueryableNt()
        {
            return _dal.AsQueryable().AsNoTracking();
        }
        public IQueryable<T> GetQueryable(Expression<Func<T, bool>> predicate)
        {
            return _dal.AsQueryable().AsNoTracking().Where(predicate);
        }

        public async Task<List<T>> GetWithCondition(Expression<Func<T, bool>> predicate)
        {
            return await _dal.Where(predicate).ToListAsync();
        }

        public async Task<List<T>> GetWithConditionNt(Expression<Func<T, bool>> predicate)
        {
            return await _dal.Where(predicate).AsNoTracking().ToListAsync();
        }



        public async Task Update(T entity)
        {
            await _dal.UpdateAsync(entity);
        }

        public async Task Update(ICollection<T> entity)
        {
            await _dal.UpdateAsync(entity);
        }

        public async Task UpdateNow(T entity)
        {
            await _dal.UpdateNowAsync(entity);
        }

        public async Task UpdateNow(ICollection<T> entity)
        {
            await _dal.UpdateNowAsync(entity);
        }

        public IBaseService<T> SetDbConnectString(string connStr)
        {
            this._connStr = connStr;
            _dal.ChangeDatabase(connStr);
            return this;
        }

        public async Task<bool> Exist(Expression<Func<T, bool>> predicate)
        {
            return await _dal.AnyAsync(predicate);
        }

        public async Task SaveChangeNow()
        {
            await this._dal.SaveNowAsync();
        }
    }
}
