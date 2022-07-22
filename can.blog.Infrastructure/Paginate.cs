using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace can.blog.Infrastructure
{
    public static class PagePaginate
    {
       

        public static IQueryable<TItem> Paginate<TItem>(this IQueryable<TItem> query, int pageIndex, int pageSize)
        {
            return pageIndex == 1 ? query.Take(pageSize) : query.Skip((pageIndex - 1) * pageSize).Take(pageSize);
        }

    }
}
