using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.DataModel.Entity
{
    public class DataEntity: BaseEntity
    {
        public int CreateUser { get; set; }
        public DateTime CreateTime { get; set; }
        public int? UpdateUser { get; set; }
        public DateTime? UpdateTime { get; set; }
    }
}