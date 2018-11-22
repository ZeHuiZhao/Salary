namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("User")]
    public partial class User
    {
        public int ID { get; set; }

        public int? DepartmentID { get; set; }

        [StringLength(50)]
        public string TrueName { get; set; }

        [StringLength(50)]
        public string PhoneNum { get; set; }

        [StringLength(200)]
        public string headimage { get; set; }

        public DateTime? JoinDate { get; set; }

        public int? IsExamine { get; set; }

        [StringLength(50)]
        public string BirthdayType { get; set; }

        public DateTime? Birthday { get; set; }

        public DateTime? CompleteDate { get; set; }

        public DateTime? CreateDate { get; set; }

        public int? NoAbnormal { get; set; }

        public DateTime? LeaveDate { get; set; }

        public int? Status { get; set; }

        [StringLength(500)]
        public string UserGuid { get; set; }
    }
}
