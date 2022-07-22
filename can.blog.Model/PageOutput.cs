using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Model
{
    public class PageOutput<T>
    {
        public int TotalPage { get; set; }
        public int Page { get; set; }
        public IList<T> PageData { set; get; }

        public PageOutput(int page, int totalPage, IList<T> data)
        {
            TotalPage = totalPage;
            Page = page;
            PageData = data;
        }
    }
}
