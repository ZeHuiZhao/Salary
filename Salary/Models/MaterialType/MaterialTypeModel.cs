using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.MaterialType
{
    public class MaterialTypeModel
    {
        public int RowNum { get; set; }
        public string TypeName { get; set; }
        public int Id { get; set; }
        public string CoverImg { get; set; }
        public int IsActive { get; set; }
        public string IsActiveName { get; set; }
    }
}