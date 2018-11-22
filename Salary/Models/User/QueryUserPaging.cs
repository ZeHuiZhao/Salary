using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.User
{
    public class QueryUserPaging : BasePaging
    {
        /// <summary>
        /// 用户列表
        /// </summary>
        public List<UserinfoDto> UserList { get; set; }
    }
}