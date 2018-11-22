using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Salary.Models.Report
{
    public class ChannelYYDto
    {
        public string MaterialTitle { get; set; }
        public string MaterialTypeName { get; set; }
        public string TrueName { get; set; }
        public string ShareCount { get; set; }
        public string BrowseCount { get; set; }
        public string EnrollCount { get; set; }
        public string ParticipateCount { get; set; }
        public string LastTime { get; set; }

    }

    public class ChannelYYDtoTemp
    {
        public int Id { get; set; }
        public int SalesId { get; set; }
        public string MaterialTitle { get; set; }
        public string MaterialTypeName { get; set; }
        public string TrueName { get; set; }
        public int ShareCount { get; set; }
        public int BrowseCount { get; set; }
        public int EnrollCount { get; set; }
        public int ParticipateCount { get; set; }
        public int AllShareCount { get; set; }
        public int AllBrowseCount { get; set; }
        public int AllEnrollCount { get; set; }
        public int AllParticipateCount { get; set; }
        public DateTime LastTime { get; set; }
    }
}