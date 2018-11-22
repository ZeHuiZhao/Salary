using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Common
{
    public class TaxHelper
    {
        static Dictionary<decimal, decimal> dictTax = new Dictionary<decimal, decimal>() {
            {0.03m,0m }
            ,{0.1m,210m }
            ,{0.2m,1410m }
            ,{0.25m,2660m }
            ,{0.3m,4410m }
            ,{0.35m,7160m }
             ,{0.45m,15160m }

        };
        public static decimal Compute(decimal value)
        {
            //ROUND(MAX((PA1-5000)*{0.03,0.1,0.2,0.25,0.3,0.35,0.45}-{0,210,1410,2660,4410,7160,15160},0),2)
            var tmp = value - 5000;
            if (tmp <=0)
                return 0;
            var max= dictTax.Select(kv => tmp * kv.Key - kv.Value).Max();
            return Math.Round(max,2);
        }
    }
}