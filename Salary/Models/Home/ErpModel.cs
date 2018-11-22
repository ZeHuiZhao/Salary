using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Home
{
    public class ErpModel
    {
        /*
         "CompanyID": 1,
        "CompanyName": "中力知识科技",
        "DepartmentID": 128,
        "DepartmentName": "商业智能集群",
        "UserID": 247,
        "UserName": "朱凡",
        "UserHeadImage": "http://usercenter.zhongliko.com/headimage/4b438076d26a4bcb965420f4c14e47f7.png",
        "UserStatus": "1",
        "UserPhone": "13924679577",
        "UserGuid": "B08F386B-4E1A-4A95-8E24-D470D8AB567C",
        "CreateDate": "2018-09-07 17:52:53",
        "UserPassword": "EnFLyjrqxgA="
             */
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserHeadImage { get; set; }
        public string UserPassword { get; set; }
        public DateTime CreateDate { get; set; }
        //public int CompanyID { get; set; }
        //public string CompanyName { get; set; }
        //public int DepartmentID { get; set; }
        //public string DepartmentName { get; set; }
        //public int UserID { get; set; }
        //public int UserStatus { get; set; }
        //public string UserGuid { get; set; }


    }
}