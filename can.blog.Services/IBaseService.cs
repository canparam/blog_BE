using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services
{
    public interface IBaseService<T> where T : class
    {
        T GetById(string id);
        T GetById(int id);
        Task<T> GetAsyncById(string id);
        Task<T> GetAsyncById(int id);
        IQueryable<T> GetAll();
        IList<T> GetListAllAsync();
        IQueryable<T> Query(Expression<Func<T, bool>> filter);
        T Update(T entity);
        List<T> UpdateMulti(List<T> listItem);
        T Insert(T entity);
        Task<T> InsertAsync(T entity);
        List<T> InsertMulti(List<T> entity);
        bool Delete(T entity);
        bool Delete(dynamic id);
        bool DeleteMulti(List<T> entity);
        T Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        List<T> FindAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task<T> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);
        Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes);


    }
}
