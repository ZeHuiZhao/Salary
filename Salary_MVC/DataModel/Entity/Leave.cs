namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Leave")]
    public partial class Leave
    {
        public int ID { get; set; }

        public int? UserID { get; set; }

        public DateTime? CreateTime { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        [StringLength(50)]
        public string Day { get; set; }

        [StringLength(50)]
        public string Hours { get; set; }

        public DateTime? FinalTime { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        [StringLength(50)]
        public string TypeName { get; set; }

        public int? Status { get; set; }

        [StringLength(500)]
        public string ExamineNote { get; set; }

        public int? AnnualLeaveDeal { get; set; }
    }
}
