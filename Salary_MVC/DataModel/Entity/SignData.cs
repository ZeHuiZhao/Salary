namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SignData")]
    public partial class SignData
    {
        public int ID { get; set; }

        [StringLength(50)]
        public string SchedulID { get; set; }

        public int? UserID { get; set; }

        public DateTime? SignInTiem { get; set; }

        public DateTime? SignOutTime { get; set; }

        [StringLength(100)]
        public string SinginType { get; set; }

        [StringLength(100)]
        public string SingoutType { get; set; }

        [StringLength(50)]
        public string SignInSchedulAttendLocationID { get; set; }

        [StringLength(50)]
        public string SignOutSchedulAttendLocationID { get; set; }

        public int? IsAbnormal { get; set; }

        [StringLength(50)]
        public string EffectiveTime { get; set; }

        public DateTime? TimeDay { get; set; }

        /// <summary>
        /// ���/ʱ
        /// </summary>
        [StringLength(50)]
        public string Leave { get; set; }
        /// <summary>
        /// �ٵ�/ʱ
        /// </summary>
        [StringLength(50)]
        public string Belate { get; set; }
        /// <summary>
        /// ����/ʱ
        /// </summary>
        [StringLength(50)]
        public string Outside { get; set; }
        /// <summary>
        /// ���Ǵ�
        /// </summary>
        [StringLength(50)]
        public string ForgotPunch { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        [StringLength(50)]
        public string LeaveEarly { get; set; }

        public int? IsComplete { get; set; }
    }
}
