using can.blog.Data.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Services.Implementations
{
    public class BaseService<T> : IBaseService<T> where T : class
    {
        protected DbSet<T> Dbset;
        private readonly BlogDbContext _dbContext;
        public BaseService(BlogDbContext db)
        {
            _dbContext = db;
            Dbset = _dbContext.Set<T>();
        }
        public bool Delete(T entity)
        {
            try
            {
                var entry = _dbContext.Entry(entity);
                entry.State = EntityState.Deleted;
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool Delete(dynamic id)
        {
            var entity = GetById(id);
            if (entity == null)
            {
                return false;
            }

            Delete(entity);
            return true;
        }

        public bool DeleteMulti(List<T> entity)
        {
            try
            {
                foreach (var item in entity)
                {
                    Delete(item);
                }
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public T Find(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Dbset.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(expression);
            return query.FirstOrDefault();
        }
        public Task<T> FindAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Dbset.AsQueryable();

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            query = query.Where(expression);
            return query.FirstOrDefaultAsync();
        }
        public List<T> FindAll(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Dbset.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return query.ToList();
        }
        public Task<List<T>> FindAllAsync(Expression<Func<T, bool>> expression, params Expression<Func<T, object>>[] includes)
        {
            var query = Dbset.AsQueryable();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (expression != null)
            {
                query = query.Where(expression);
            }
            return query.ToListAsync();
        }

        public IQueryable<T> GetAll()
        {
            return Dbset.AsQueryable();
        }

        public async Task<T> GetAsyncById(string id)
        {
            return await Dbset.FindAsync(id);
        }
        public async Task<T> GetAsyncById(int id)
        {
            return await Dbset.FindAsync(id);
        }

        public T GetById(string id)
        {
            return Dbset.Find(id);
        }

        public IList<T> GetListAllAsync()
        {
            return Dbset
                .AsQueryable()
                .ToList();
        }
        public T Insert(T entity)
        {
            Dbset.Add(entity);
            return entity;
        }

        public async Task<T> InsertAsync(T entity)
        {
            await Dbset.AddAsync(entity);
            return entity;
        }

        public List<T> InsertMulti(List<T> entity)
        {
            foreach (var item in entity)
            {
                Dbset.Add(item);
            }
            return entity;
        }

        public IQueryable<T> Query(Expression<Func<T, bool>> filter)
        {
            return Dbset.Where(filter);
        }

        public T Update(T entity)
        {
            var dbEntityEntry = _dbContext.Entry(entity);
            if (dbEntityEntry.State == EntityState.Detached)
            {
                Dbset.Attach(entity);
            }

            dbEntityEntry.State = EntityState.Modified;
            return entity;
        }

        public List<T> UpdateMulti(List<T> listItem)
        {
            foreach (var item in listItem)
            {
                Update(item);
            }
            return listItem;
        }

        public T GetById(int id)
        {
            return Dbset.Find(id);
        }
    }
}
