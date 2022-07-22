using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace can.blog.Data.Repository
{
    public interface IUnitOfWork
    {
        Task<bool> CompleteAsync();

        bool Complete();

        IDbContextTransaction BeginTransaction();
    }
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BlogDbContext _context;

        public UnitOfWork(BlogDbContext context)
        {
            this._context = context;
        }


        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }

        public bool Complete()
        {
            return _context.SaveChanges() > 0;
        }

        public async Task<bool> CompleteAsync()
        {
            return await this._context.SaveChangesAsync() > 0;
        }
    }

}
