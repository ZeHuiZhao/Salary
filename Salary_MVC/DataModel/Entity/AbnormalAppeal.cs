namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("AbnormalAppeal")]
    public partial class AbnormalAppeal
    {
        public int ID { get; set; }

        [StringLength(200)]
        public string WorkAddress { get; set; }

        [StringLength(100)]
        public string AppealType { get; set; }

        [StringLength(200)]
        public string Reason { get; set; }

        public DateTime? FinalTime { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? UserID { get; set; }

        public int? SignDataID { get; set; }

        public DateTime? TimeDay { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public int? Status { get; set; }
    }
}
