namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("WagesCode")]
    public partial class WagesCode
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        [StringLength(20)]
        public string identifycode { get; set; }

        public int? Status { get; set; }

        public DateTime? CodeCreateTime { get; set; }

        [StringLength(50)]
        public string TimeMonth { get; set; }
    }
}
