using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary_MVC.Models
{
    public class MenuDto
    {
        public Guid id { get; set; }
        public string text { get; set; }
        public string url { get; set; }
        public string urlType { get; set; }
        public string icon { get; set; }
        public bool isHeader { get; set; }
        public List<MenuDto> children { get; set; }
        public string targetType { get; set; }
    }
}