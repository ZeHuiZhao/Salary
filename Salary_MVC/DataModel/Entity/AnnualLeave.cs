namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AnnualLeave")]
    public partial class AnnualLeave
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public int? LeaveID { get; set; }

        public DateTime? BeginTime { get; set; }

        public DateTime? FinishTime { get; set; }

        public double? Hours { get; set; }

        [StringLength(500)]
        public string Note { get; set; }

        public DateTime? CreatTime { get; set; }

        [StringLength(50)]
        public string WorkYear { get; set; }

        public int? Status { get; set; }
    }
}
