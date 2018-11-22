using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models
{
    public class ErpResult
    {
        public int status { get; set; }
        public string message { get; set; }
        public int primary_id { get; set; } = 0;
    }
}