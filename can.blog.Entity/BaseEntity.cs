using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace can.blog.Entity
{
    public class BaseEntity
    {
        public string ModifiedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        [DefaultValue(0)]
        public int IsDel { get; set; }
    }
}
