using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class MaterialWechatModel
    {
        public int Id { get; set; }
        public string MaterialTitle { get; set; }
        public string CoverImg { get; set; }
        public string CreateTime { get; set; }
        public int Share { get; set; }
        public int Browse { get; set; }
        public int Enroll { get; set; }
    }
}