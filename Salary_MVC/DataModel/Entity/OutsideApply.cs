namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("OutsideApply")]
    public partial class OutsideApply
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public DateTime? FinalTime { get; set; }

        [StringLength(50)]
        public string Day { get; set; }

        [StringLength(50)]
        public string Hours { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? UserID { get; set; }

        public DateTime? TimeDay { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        public int? Status { get; set; }
    }
}
