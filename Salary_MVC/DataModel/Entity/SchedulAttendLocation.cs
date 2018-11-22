namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SchedulAttendLocation")]
    public partial class SchedulAttendLocation
    {
        public int ID { get; set; }

        public int? SchedulID { get; set; }

        public int? AttendLocationID { get; set; }
    }
}
