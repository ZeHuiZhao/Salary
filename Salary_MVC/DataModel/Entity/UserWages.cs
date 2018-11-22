namespace Salary_MVC.DataModel
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserWages
    {
        public int ID { get; set; }

        [StringLength(20)]
        public string UserID { get; set; }

        [StringLength(50)]
        public string UserName { get; set; }

        [StringLength(50)]
        public string Days { get; set; }

        [StringLength(50)]
        public string BaseWag { get; set; }

        [StringLength(500)]
        public string Other { get; set; }

        [StringLength(50)]
        public string TotalAmount { get; set; }

        [StringLength(50)]
        public string SocialSecurity { get; set; }

        [StringLength(50)]
        public string HousingFund { get; set; }

        [StringLength(50)]
        public string PersonalTax { get; set; }

        [StringLength(50)]
        public string Othercutpay { get; set; }

        [StringLength(500)]
        public string CutPayRemark { get; set; }

        [StringLength(50)]
        public string CutPayTitle { get; set; }

        [StringLength(50)]
        public string PayTrue { get; set; }

        public DateTime? TimeMonth { get; set; }

        [StringLength(50)]
        public string JobSubsidies { get; set; }

        [StringLength(50)]
        public string JobLevel { get; set; }
    }
}
