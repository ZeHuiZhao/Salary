namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Schedul")]
    public partial class Schedul
    {
        public int ID { get; set; }

        [StringLength(100)]
        public string Name { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? IsAll { get; set; }

        [StringLength(50)]
        public string UserID { get; set; }

        public DateTime? MstartTime { get; set; }

        public DateTime? MendTime { get; set; }

        public DateTime? AstartTime { get; set; }

        public DateTime? AendTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TimeDay { get; set; }

        public int? AdminID { get; set; }
    }
}
