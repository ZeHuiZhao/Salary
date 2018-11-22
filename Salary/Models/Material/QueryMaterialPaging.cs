using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class QueryMaterialPaging:BasePaging
    {
        public List<MaterialModel> MaterialList { get; set; }
    }
}