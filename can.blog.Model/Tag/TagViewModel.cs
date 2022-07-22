using System;
using System.Collections.Generic;
using System.Text;

namespace can.blog.Model.Tag
{
    public class TagViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public int? CountTotalPost { get; set; }
        public DateTime CreateCreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
