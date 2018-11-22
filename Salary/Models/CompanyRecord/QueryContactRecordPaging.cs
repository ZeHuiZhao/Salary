using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.ComapnyRecord
{
    public class QueryContactRecordPaging:BasePaging
    {
        public List<CompanyRecordDto> CompanyRecordList { get; set; }
    }
}