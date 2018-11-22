using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class QueryWechatMaterialPaging:BasePaging
    {
        public List<MaterialWechatModel> MaterialList { get; set; }
    }
}