﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace eShopApp.DataAccessLayer.Repository.GenericRepository
{
    public interface IGenericRepository<T> where T : class
    {
        //x=>x.id==1
        IEnumerable<T> GetAll(Expression<Func<T,bool>>? expression=null,string? includeProperties = null);
        T GetT(Expression<Func<T, bool>>? expression = null, string? includeProperties = null);

        void Add(T entity);
        void Delete(T entity);
        void DeleteRange(IEnumerable<T> entity);
    }
}
