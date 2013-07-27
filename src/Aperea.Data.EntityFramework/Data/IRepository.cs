﻿using System;
using System.Linq;
using System.Linq.Expressions;

namespace Aperea.Data
{
    public interface IRepository<T>
        where T : class
    {
        IQueryable<T> Entities { get; }
        void Add(T entity);
        void Remove(T entity);
        void SaveChanges();
        IQueryable<T> Include(params string[] paths);
        IQueryable<T> Include<TProperty>(params Expression<Func<T, TProperty>>[] expressions);
        void Update(T person);
    }
}