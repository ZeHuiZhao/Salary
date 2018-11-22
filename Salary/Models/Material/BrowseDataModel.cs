using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Material
{
    public class BrowseDataModel
    {
        public string MaterialTitle { get; set; }
        public string CoverImg { get; set; }
        public string MaterialTypeName { get; set; }
        public string LastShareTimeToString { get; set; }

        public DateTime LastShareTime { get; set; }
        public int BrowseCount { get; set; }
    }
}