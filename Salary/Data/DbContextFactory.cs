using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using Salary.DataModel.Entity;

namespace Salary.Data
{
    /// <summary>
    /// 获取DbContext(保证一次请求中DbContext唯一)
    /// </summary>
    public class DbContextFactory
    {
        public static DbContext GetCurrentDbContext()
        {
            object _lock = new object();
            DbContext dbContext = HttpContext.Current.Items["DbContext"] as DbContext;

            if (dbContext == null)
            {
                lock (_lock)
                {
                    if (dbContext == null)
                    {

                        dbContext = new YYDbContext();
                        HttpContext.Current.Items["DbContext"] = dbContext;
                    }
                }
            }

            return dbContext;

        }
    }
}